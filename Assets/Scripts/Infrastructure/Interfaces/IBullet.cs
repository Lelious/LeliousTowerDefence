using UnityEngine;

public interface IBullet
{
    public void SetTarget(IDamagable damagable);
    public Transform Transform();
    public Bullet GetBullet();
}

public interface IPoollableBullet : IBullet
{
    public void SetPool(BulletPool pool);
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
    public bool IsFree();
}
