using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage, DamageSource source = DamageSource.Normal);
    public IEffectable GetEffectable();
    public bool CanBeAttacked();
    EnemyHitPoint HitPoint();
    Transform GetOrigin();
    public GameObject gameObject { get; }
}
