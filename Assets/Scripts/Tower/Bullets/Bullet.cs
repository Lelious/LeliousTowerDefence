using System.Collections.Generic;
using Assets.Scripts.Tower.TowerAbilities;
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
    protected BuffService _buffService;
    protected IEffectable _effectable;
    protected EffectData _effectData;
    protected float _flyingSpeed;
    protected float _curvature;
    protected PoolService _poolService;

    private EnemyHitPoint _enemyHitPoint;
    private Shooter _shooter;
    private bool _callback;
    private IPoollableBullet _iPoollable;
    private BulletType _type;
    private Vector3 _middlePoint;
    private Vector3 _startPoint;
    private Vector3 _endPoint;    
    private float _flyingProgress;    
    private float _speedIncreacement;
    private bool _onTarget;
    private bool _onFlying;

    private List<TowerAbility> _abilitiesList;

    public void ReturnToPool() => _poolService.AddBulletToPool(_type, _iPoollable);
    public void SetEffectable(IEffectable effectable) => _effectable = effectable;
    public void SetBuffService(BuffService service) => _buffService = service;
    public int CalculateDamage() => Random.Range(_minDamage, _maxDamage + 1);
    public void SetEndPoint(Vector3 position) => _endPoint = position;
    public void SetEffectData(EffectData data) => _effectData = data;
    public void SetBulletType(BulletType type) => _type = type;
    public void SetInnactive() => gameObject.SetActive(false);
    public void SetActive() => gameObject.SetActive(true);
    public void DestroyBullet() => Destroy(gameObject);
    public virtual void ApplySpecialEffects() { }
    public virtual void BulletAchieveTarget() { }
    public BulletType GetBulletType() => _type;
    public virtual void BulletReadyToFly() { }

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
        _endPoint = _damagable.GetOrigin().position;
    }

    public void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition, Shooter shooter, bool callback)
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
        _shooter = shooter;
        _callback = callback;
        _abilitiesList = data.Abilities;

        if (_impactOnHit == null)
        {
            _impactOnHit = Instantiate(data.ImpactOnHit);
            _impactOnHit.SetActive(false);
        }

        ResetPath(startPosition);
        BulletReadyToFly();
    }

    public void ApplyDamage(int damage = 0)
    {
        bool processedDamage = false;

        if (_abilitiesList.Count > 0)
        {
            foreach (var ability in _abilitiesList)
            {
                if (ability.Chance > Random.Range(0.0f, 1.0f))
                {
                    if (ability.AttackModifier)
                    {
                        processedDamage = true;

                        if ((Component)_damagable != null)
                        {
                            _damagable.TakeDamage(damage == 0 ? (int)(CalculateDamage() * ability.DamageMultiplier) : damage, ability.DamageSource);
                        }
                    }
                    if (ability.Data != null && ability.AppliedTarget == AppliedTarget.Enemy)
                    {
                        var effectable = _damagable.GetEffectable();
                        _buffService.ApplyEffect(effectable, new Debuff(effectable, ability.Data));
                    }
                }
            }
        }

        if(!processedDamage)
        {
            if ((Component)_damagable != null)
            {
                _damagable.TakeDamage(damage == 0 ? CalculateDamage() : damage);
            }
        }

        CreateHitImpact(Vector3.zero);

        if (_callback) _shooter.RegisterAim();
    }

    public void SetBulletPool(PoolService pool, bool addToPool = true)
    {
        _iPoollable = GetComponent<IPoollableBullet>();
        _poolService = pool;
    }

    protected void CreateHitImpact(Vector3 upScale)
    {
        Vector3 position = _effectable == null ? transform.position + upScale : _effectable.GetOrigin().position + upScale;
        _impactOnHit.SetActive(false);
        _impactOnHit.transform.position = position;
        _impactOnHit.SetActive(true);
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
                    _endPoint = _enemyHitPoint.transform.position;
                }
            }
        }

        _middlePoint = new Vector3(
            (_startPoint.x + _endPoint.x) / 2,
            (_startPoint.y + _endPoint.y) / 2 + Vector3.Distance(_startPoint, _endPoint) * _curvature,
            (_startPoint.z + _endPoint.z) / 2);

        if (_flyingProgress < 0.99f)
        {
            transform.position = Bezier.GetSquarePoint(
                _startPoint,
                _middlePoint,
                _endPoint,
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
               _endPoint,
               _flyingProgress));
    }

    public void Destroy() => Destroy(gameObject);
}
