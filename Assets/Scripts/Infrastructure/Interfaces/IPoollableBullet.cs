using UnityEngine;

public interface IPoollableBullet : IPoollableObject
{
    public BulletType GetBulletType();
    public void SetBulletType(BulletType type);
    public void SetEffectable(IEffectable effectable);
    public void SetEffectData(EffectData data);   
    public void DestroyBullet();
    public void SetBuffService(BuffService service);
    public void SetBulletParameters(TowerStats data, EnemyPool enemyPool, Vector3 startPosition, Shooter shooter, bool callback = false);
    public void SetTarget(IDamagable damagable);
    public void SetEndPoint(Vector3 position);
    public void ResetPath(Vector3 position);
}
