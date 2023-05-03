using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

public class ShurikenBullet : Bullet, IPoollableBullet
{
    [SerializeField] private GameObject _impactOnHitPrefab;

    private EnemyPool _enemyPool;
    private IPoollableBullet _iPoollable;
    private IDamagable _damagable;
    private EnemyHitPoint _enemyHitPoint;
    private PoolService _poolService;
    private GameObject _impactOnHit;
    private Transform _hitPointTransform;
    private Quaternion _rotation;
    private float _flyingProgress;
    private float _returningTime;
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
    public override Bullet GetBulletType() => this;


    public override void SetBulletPool(PoolService pool, bool addToPool = true)
    {
        _poolService = pool;        
        _iPoollable = GetComponent<IPoollableBullet>();

        if (addToPool)       
            ReturnToPool();       
    }

    public override void SetBulletParameters(TowerData data, EnemyPool enemyPool, Vector3 startPosition)
    {
        _damage = Random.Range(data.MinimalDamage, data.MaximumDamage + 1);
        _flyingSpeed = data.ProjectileSpeed;
        _returningTime = data.ProjectileParentingTime;
        _targetsCount = data.RicochetteCount;
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
            if (_enemyHitPoint != null)
            {
                transform.LookAt(_enemyHitPoint.transform);

                if (Vector3.Distance(transform.position, _enemyHitPoint.transform.position) > 0.1f)
                {
                    transform.position = Vector3.Lerp(transform.position, _enemyHitPoint.transform.position, _flyingProgress);
                    _flyingProgress += _flyingSpeed;
                    _flyingSpeed += 0.0001f;                   
                }

                else
                {
                    if (!_isDealDamage)
                    {
                        _isDealDamage = true;
                        _damagable.TakeDamage(_damage);

                        if (_impactOnHit != null)
                        {
                            _impactOnHit.transform.position = _hitPointTransform.position;
                            _impactOnHit.SetActive(true);
                        }

                        if (_currentTarget < _targetsCount)
                        {
                            Debug.Log("Yes asdasd");
                            var nextDamagable = FindNextTarget();

                            if (nextDamagable != null)
                            {
                                SetTarget(nextDamagable);
                                _currentTarget++;
                                //var nextDmg = _damage * 0.7f;
                                _damage = (int)nextDmg < 0 ? _damage = 1 : _damage = (int)nextDmg;
                            }
                        }
                        else
                        {
                            StartCoroutine(ReturnToPoolRoutine());
                        }
                    }
                }
            }
            else
            {
                StartCoroutine(ReturnToPoolRoutine());
            }

            yield return null;
        }
    }

    private IDamagable FindNextTarget()
    {
        return _enemyPool.GetEnemyFromDistance(transform, 2f, _damagable);
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_returningTime);
        _enemyHitPoint.RemoveAttachedBulletFromHitPoint(this);
        ReturnToPool();
    }
}
