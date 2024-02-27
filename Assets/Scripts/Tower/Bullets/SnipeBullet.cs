using System.Collections;
using UnityEngine;

public class SnipeBullet : Bullet
{
    public override void ApplySpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletAchieveTarget()
    {
        ApplyDamage();
        ReturnToPool();
    }

    public override void BulletReadyToFly()
    {
        throw new System.NotImplementedException();
    }
}
