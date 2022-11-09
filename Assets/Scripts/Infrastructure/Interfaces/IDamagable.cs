using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(float damage);
    public bool CanBeAttacked();
    EnemyHitPoint HitPoint();
}
