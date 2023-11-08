using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ShurikenBullet : Bullet, IPoollableBullet
{
    [SerializeField] private GameObject _impactOnHitPrefab;
    [SerializeField] private List<IDamagable> _hittedTargets = new List<IDamagable>();

    private EnemyPool _enemyPool;
    private IPoollableBullet _iPoollable;
    private IDamagable _damagable;
    private EnemyHitPoint _enemyHitPoint;
    private PoolService _poolService;
    private GameObject _impactOnHit;
    private Transform _hitPointTransform;
    private Vector3 _direction;
    private float _distance;
    private float _flyingProgress;
    private float _flyingSpeed;
    private int _targetsCount;
    private int _currentTarget;
    private int _damage;
    private bool _onFlying;
    private bool _isDealDamage;

    public override void FlyOnTarget() => StartCoroutine(ArrowFlyingRoutine());
    public override void ReturnBulletToPool() => ReturnToPool();
    public void SetInnactive() => gameObject.SetActive(false);
    public void SetActive() => gameObject.SetActive(true);
    public void DestroyBullet() => Destroy(this);
    public override Bullet GetBulletType() => this;


    public override void SetBulletPool(PoolService pool, bool addToPool = true)
    {
        _poolService = pool;        
        _iPoollable = GetComponent<IPoollableBullet>();

        if (addToPool)       
            ReturnToPool();       
    }

    public override void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition)
    {
        _damage = Random.Range(data.MinimalDamage, data.MaximumDamage + 1);
        _flyingSpeed = data.ProjectileSpeed;
        _targetsCount = 5;
        _currentTarget = 0;
        _enemyPool = enemyPool;

        transform.position = startPosition;

        if (_impactOnHitPrefab == null)       
            return;       
        if (_impactOnHit != null)       
            return;        
        _impactOnHit = Instantiate(_impactOnHitPrefab);
        _impactOnHit.SetActive(false);
    }     

    public void ReturnToPool()
    {
        StopAllCoroutines();

        _damagable = null;
        _enemyHitPoint = null;
        _isDealDamage = false;
        _flyingProgress = 0f;
        _onFlying = false;
        _currentTarget = 0;
        _hittedTargets.Clear();
        _poolService.AddBulletToPool(typeof(ShurikenBullet), _iPoollable);
    }

    public override void SetTarget(IDamagable damagable)
    {
        _isDealDamage = false;
        _damagable = damagable;
        _enemyHitPoint = _damagable.HitPoint();
        _hitPointTransform = _enemyHitPoint.transform;
        _flyingProgress = 0f;
    }

    private IEnumerator ArrowFlyingRoutine()
    {
        _onFlying = true;

        while (_onFlying)
        {
            if (_enemyHitPoint != null && _damagable != null && _damagable.CanBeAttacked())
            {
                _direction = _enemyHitPoint.transform.position - transform.position;
                _distance = _direction.magnitude;

                if (_distance > 0.5f)
                {
                    transform.Translate((_direction / _distance) * _flyingProgress, Space.Self);
                    _flyingProgress += _flyingSpeed;
                    _flyingSpeed += 0.0003f;                   
                }

                else
                {
                    if (!_isDealDamage)
                    {
                        _isDealDamage = true;
                        _damagable.TakeDamage(_damage);

                        if (!_hittedTargets.Contains(_damagable))
                        {
                            _hittedTargets.Add(_damagable);
                        }

                        if (_impactOnHit != null)
                        {
                            _impactOnHit.transform.position = _hitPointTransform.position;
                            _impactOnHit.SetActive(true);
                        }

                        if (_currentTarget < _targetsCount)
                        {
                            var nextDamagable = FindNextTarget();

                            if (nextDamagable != null)
                            {
                                SetTarget(nextDamagable);
                                _currentTarget++;
                                var nextDmg = _damage * 0.7f;
                                _damage = (int)nextDmg < 0 ? _damage = 1 : _damage = (int)nextDmg;
                            }
                            else
                            {
                                ReturnToPool();
                            }
                        }
                        else
                        {
                            ReturnToPool();
                        }
                    }
                }
            }
            else
            {
                _damagable = FindNextTarget();

                if (_damagable == null)
                {
                    ReturnToPool();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IDamagable FindNextTarget()
    {
        var availableTargets = _enemyPool.GetEnemiesFromDistance(transform, 5f, _damagable);
        List<IDamagable> correctTargets = new List<IDamagable>();

        if (availableTargets.Count == 0)
        {
            return null;
        }

        else
        {
            foreach (var target in availableTargets)
            {
                if (!_hittedTargets.Contains(target))
                {
                    correctTargets.Add(target);
                }
            }

            if (correctTargets.Count > 0)
            {
                correctTargets = correctTargets.OrderBy((x) => (x.GetOrigin().position - transform.position).sqrMagnitude).ToList();
                return correctTargets[0];
            }

            else
            {
                return availableTargets.FirstOrDefault();
            }
        }    
    }
}
