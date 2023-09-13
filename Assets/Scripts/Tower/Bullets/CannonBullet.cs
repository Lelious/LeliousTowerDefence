using UnityEngine;
using DG.Tweening;

public class CannonBullet : Bullet, IPoollableBullet
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TrailRenderer _trail;

    private IPoollableBullet _iPoollable;
    private PoolService _pool;
    private GameObject _explosion;
    private Transform _targetTransform;
    private Collider[] _hits = new Collider[10];
    private Tween _tween;
    private IDamagable _damagable;
    private int _damage;
    private float _explosionRadius;

    public override void ReturnBulletToPool() => ReturnToPool();
    public void SetInnactive() => gameObject.SetActive(false);
    public void SetActive() => gameObject.SetActive(true);
    public void DestroyBullet() => Destroy(this);
    public Transform Transform() => transform;
    public override Bullet GetBulletType() => this;

    public override void FlyOnTarget()
    {
        _renderer.enabled = true;

        if (_explosion == null)
        {
            _explosion = Instantiate(_explosionPrefab);
            _explosion.SetActive(false);
        }
        else       
            _explosion.SetActive(false);
        
        if (_targetTransform != null)
            _tween = transform.DOJump(_targetTransform.position, 5f, 1, (_targetTransform.position - transform.position).magnitude / 8f).OnComplete(()=> Explode()).SetEase(Ease.Linear);       
    }

    public override void SetBulletParameters(TowerData data, EnemyPool enemyPool, Vector3 startPosition)
    {
        _damage = Random.Range(data.MinimalDamage, data.MaximumDamage + 1);
        _explosionRadius = data.ExplosionRadius;
        _explosionPrefab.transform.localScale = Vector3.one * _explosionRadius;
        transform.position = startPosition;
    }

    public override void SetTarget(IDamagable damagable)
    {
        _damagable = damagable;
        _targetTransform = _damagable.GetOrigin();
    }

    public void ReturnToPool()
    {
        _tween.Kill();
        _pool.AddBulletToPool(typeof(CannonBullet), _iPoollable);
    }

    private void Explode()
    {
        ClearColliders();
        _renderer.enabled = false;
        _explosion.transform.position = transform.position;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _hits, _mask);

        for (int i = 0; i < numColliders; i++)
        {
            var damageble = _hits[i].GetComponent<IDamagable>();

            if (damageble != null)
            {
                damageble.TakeDamage(_damage);
            }
        }
        ReturnToPool();
        _explosion.SetActive(true);
    }

    private void ClearColliders()
    {
        for (int i = 0; i < _hits.Length - 1; i++)       
            _hits[i] = null;     
    }

    public override void SetBulletPool(PoolService pool, bool addToPool = true)
    {
        _iPoollable = GetComponent<IPoollableBullet>();
        _pool = pool;

        if (addToPool)                
            ReturnToPool();
    }
}

