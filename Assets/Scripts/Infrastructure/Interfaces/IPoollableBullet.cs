using UnityEngine;

public interface IPoollableBullet
{
    public BulletType GetBulletType();
    public void SetBulletType(BulletType type);
    public void SetEffectable(IEffectable effectable);
    public void SetEffectData(EffectData data);
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
    public void DestroyBullet();
    public void SetBulletPool(PoolService pool, bool addToPool = true);
    public void SetBuffService(BuffService service);
    public void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition, Shooter shooter, bool callback = false);
    public void SetTarget(IDamagable damagable);
    public void SetEndPoint(Vector3 position);
    public void ResetPath(Vector3 position);
}
