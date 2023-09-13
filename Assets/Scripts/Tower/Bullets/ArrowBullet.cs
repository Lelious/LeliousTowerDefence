using System.Collections;
using UnityEngine;

public class ArrowBullet : Bullet, IPoollableBullet
{
    [SerializeField] private GameObject _impactOnHitPrefab;

    private IPoollableBullet _iPoollable;
    private IDamagable _damagable;
    private EnemyHitPoint _enemyHitPoint;
    private PoolService _poolService;
    private GameObject _impactOnHit;
    private Transform _hitPointTransform;
    private Vector3 _endPoint;
    private Vector3 _offset;
    private Quaternion _rotation;
    private float _flyingProgress;
    private float _returningTime;
    private float _flyingSpeed;
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

    public override void SetBulletParameters(TowerData data, EnemyPool enemyPool, Vector3 startPosition)
    {
        _damage = Random.Range(data.MinimalDamage, data.MaximumDamage + 1);
        _flyingSpeed = data.ProjectileSpeed;
        _returningTime = data.ProjectileParentingTime;
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
        _poolService.AddBulletToPool(typeof(ArrowBullet), _iPoollable);
    }

    public override void SetTarget(IDamagable damagable)
    {
        _damagable = damagable;
        _enemyHitPoint = _damagable.HitPoint();
        _hitPointTransform = _enemyHitPoint.transform;
    }

    private IEnumerator ArrowFlyingRoutine()
    {
        _onFlying = true;

        while (_onFlying)
        {
            if (_enemyHitPoint != null)
            {
                _endPoint = _enemyHitPoint.transform.position;

                if (_enemyHitPoint.gameObject.activeInHierarchy)
                {
                    transform.LookAt(_enemyHitPoint.transform);

                    if (Vector3.Distance(transform.position, _endPoint) > 0.5f)
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
                            _damagable.HitPoint().AttachBulletToHitPoint(this);

                            if (_impactOnHit != null)
                            {
                                _impactOnHit.transform.position = _hitPointTransform.position;
                                _impactOnHit.SetActive(true);
                            }

                            _onFlying = false;
                            StartCoroutine(ReturnToPoolRoutine());
                        }
                    }
                }
                else
                {
                    StartCoroutine(ReturnToPoolRoutine());
                }
            }           
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_returningTime);
        _enemyHitPoint.RemoveAttachedBulletFromHitPoint(this);
        ReturnToPool();
    }
}
