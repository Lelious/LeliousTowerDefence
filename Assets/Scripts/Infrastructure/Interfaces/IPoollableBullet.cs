using UnityEngine;

public interface IPoollableBullet
{
    public BulletType GetBulletType();
    public void SetBulletType(BulletType type);
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
    public void DestroyBullet();
    public void SetBulletPool(PoolService pool, bool addToPool = true);
    public void SetBulletParameters(TowerStats data, EnemyPool enemyPool, BuffService buffService, Vector3 startPosition, Shooter shooter, bool callback = false);
    public void SetTarget(IDamagable damagable);
}
