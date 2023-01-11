using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);
    public bool CanBeAttacked();
    EnemyHitPoint HitPoint();
    Transform GetOrigin();
}
