using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public abstract void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition);
    public abstract void SetBulletPool(PoolService pool, bool addToPool = true);
    public abstract void SetTarget(IDamagable damagable);
    public abstract void ReturnBulletToPool();
    public abstract void FlyOnTarget();
    public abstract Bullet GetBulletType();
}
