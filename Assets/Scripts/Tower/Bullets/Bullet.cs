using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public abstract void SetBulletParameters(TowerData data);
    public abstract void SetHitPoint(EnemyHitPoint point);
    public abstract void FlyOnTarget();      
}
