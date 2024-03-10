public class IceBullet : Bullet
{
    public override void ApplySpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletAchieveTarget()
    {
        _trail.SetActive(false);
        ApplyDamage();
        ReturnToPool();
    }

    public override void BulletReadyToFly()
    {
        _trail.SetActive(true);
    }
}
