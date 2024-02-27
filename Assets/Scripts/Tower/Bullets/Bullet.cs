using UnityEngine;

public abstract class Bullet : MonoBehaviour, IPoollableBullet
{
    [SerializeField] protected GameObject _trail;

    protected GameObject _impactOnHit;    
    protected IDamagable _damagable;   
    protected float _explosionRadius;
    protected int _ricoshetteCount;
    protected EnemyPool _enemyPool;
    protected int _minDamage, _maxDamage;

    private EnemyHitPoint _enemyHitPoint;
    private PoolService _poolService;
    private IPoollableBullet _iPoollable;
    private BulletType _type;
    private Vector3 _middlePoint;
    private Vector3 _startPoint;
    private Vector3 _enemyPoint;
    private float _curvature;
    private float _flyingProgress;
    private float _flyingSpeed;
    private float _speedIncreacement;
    private bool _onTarget;
    private bool _onFlying;

    public void ReturnToPool() => _poolService.AddBulletToPool(_type, _iPoollable);
    public void SetBulletType(BulletType type) => _type = type;
    public void SetInnactive() => gameObject.SetActive(false);
    public void SetActive() => gameObject.SetActive(true);
    public void DestroyBullet() => Destroy(gameObject);
    public BulletType GetBulletType() => _type;

    public abstract void BulletAchieveTarget();
    public abstract void BulletReadyToFly();
    public abstract void ApplySpecialEffects();

    public int CalculateDamage() => Random.Range(_minDamage, _maxDamage + 1);

    public void ResetPath(Vector3 startPoint)
    {
        _startPoint = startPoint;
        transform.position = _startPoint;
        _flyingProgress = 0f;
        _onFlying = true;
    }

    public void SetTarget(IDamagable damagable)
    {
        _damagable = damagable;
        _enemyHitPoint = _damagable.HitPoint();
        _enemyPoint = _damagable.GetOrigin().position;
    }   

    public void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition)
    {
        _explosionRadius = data.ExplosionRadius;
        _minDamage = (int)(data.MinimalDamage + data.BonusAttackPower.Value);
        _maxDamage = (int)(data.MaximumDamage + data.BonusAttackPower.Value);
        _flyingSpeed = data.ProjectileSpeed;
        _curvature = data.BulletYCurvature;
        _speedIncreacement = data.ProjectileSpeedIncreacement;
        _onTarget = data.OnTarget;
        _ricoshetteCount = data.RicochetteCount;
        _enemyPool = enemyPool;

        if (_impactOnHit == null)      
            _impactOnHit = Instantiate(data.ImpactOnHit);
        _impactOnHit.SetActive(false);

        ResetPath(startPosition);
        BulletReadyToFly();
    }

    public void ApplyDamage(int damage = 0)
    {
        if ((Component)_damagable != null)
        {
            _damagable.TakeDamage(damage == 0 ? CalculateDamage() : damage);
        }

        _impactOnHit.transform.position = transform.position;
        _impactOnHit.SetActive(true);      
    }

    public void SetBulletPool(PoolService pool, bool addToPool = true)
    {
        _iPoollable = GetComponent<IPoollableBullet>();
        _poolService = pool;
    }

    private void FixedUpdate()
    {
        if (_onFlying)
        {
            MoveBullet();
            RotateBullet();
        }
    }

    private void MoveBullet()
    {
        if (_enemyHitPoint != null)
        {
            if (_onTarget)
            {
                if (_enemyHitPoint.GetActiveStatus())
                {
                    _enemyPoint = _enemyHitPoint.transform.position;
                }
            }
        }

        _middlePoint = new Vector3(
            (_startPoint.x + _enemyPoint.x) / 2,
            (_startPoint.y + _enemyPoint.y) / 2 + Vector3.SqrMagnitude(_startPoint - _enemyPoint) * _curvature,
            (_startPoint.z + _enemyPoint.z) / 2);

        if (_flyingProgress < 0.99f)
        {
            transform.position = Bezier.GetSquarePoint(
                _startPoint,
                _middlePoint,
                _enemyPoint,
                _flyingProgress);

            _flyingProgress += _flyingSpeed;
            _flyingSpeed += _speedIncreacement;
        }

        else
        {
            _onFlying = false;
            BulletAchieveTarget();
        }
    }
    private void RotateBullet()
    {
        transform.rotation = Quaternion.LookRotation(Bezier.GetLookVector3Square(
               _startPoint,
               _middlePoint,
               _enemyPoint,
               _flyingProgress));
    }   
}
