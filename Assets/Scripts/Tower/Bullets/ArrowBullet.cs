using System.Collections;
using UnityEngine;

public class ArrowBullet : Bullet, IPoollableBullet
{  
    private BulletPool _bulletPool;
    private IDamagable _damagable;
    private EnemyHitPoint _enemyHitPoint;
    private float _flyingProgress;
    private float _returningTime;
    private float _flyingSpeed;
    private int _damage;
    private bool _onFlying;
    private bool _isDealDamage;

    public void SetPool(BulletPool pool) => _bulletPool = pool;
    public void SetInnactive() => gameObject.SetActive(false);
    public bool IsFree() => !gameObject.activeInHierarchy;
    public void SetActive() => gameObject.SetActive(true);
    public Transform Transform() => transform;
    public Bullet GetBullet() => this;

    public override void SetBulletParameters(TowerData data)
    {
        _damage = Random.Range(data.MinimalDamage, data.MaximumDamage + 1);
        _flyingSpeed = data.ProjectileSpeed;
        _returningTime = data.ProjectileParentingTime;
    }

    public override void FlyOnTarget()
    {       
        StartCoroutine(ArrowFlyingRoutine());
    }

    public void ReturnToPool()
    {
        StopAllCoroutines();
        _bulletPool.Return(this);
        _isDealDamage = false;
        _flyingProgress = 0f;
        _onFlying = false;
    }

    public void SetTarget(IDamagable damagable)
    {
        _damagable = damagable;
        _enemyHitPoint = _damagable.HitPoint();
    }

    private IEnumerator ArrowFlyingRoutine()
    {
        _onFlying = true;

        while (_onFlying)
        {
            if (_enemyHitPoint != null)
            {
                transform.LookAt(_enemyHitPoint.transform);

                if (Vector3.Distance(transform.position, _enemyHitPoint.transform.position) > 0.5f)
                {
                    transform.position = Vector3.Lerp(transform.position, _enemyHitPoint.transform.position, _flyingProgress);
                    _flyingProgress += _flyingSpeed;
                }
                else
                {
                    if (!_isDealDamage)
                    {
                        _isDealDamage = true;
                        _damagable.TakeDamage(_damage);
                        _damagable.HitPoint().AttachBulletToHitPoint(this);
                        _onFlying = false;
                        StartCoroutine(ReturnToPoolRoutine());
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_returningTime);
        _enemyHitPoint.RemoveAttachedBulletFromHitPoint(this);
        ReturnToPool();
    }

    public override void SetHitPoint(EnemyHitPoint point)
    {
        throw new System.NotImplementedException();
    }
}
