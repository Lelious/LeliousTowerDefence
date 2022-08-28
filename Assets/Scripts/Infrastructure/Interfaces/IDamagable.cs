using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);
    public bool CanBeAttacked();
    public void ApplyBullet(Bullet bullet);
    public void RemoveBullet(Bullet bullet);
}
