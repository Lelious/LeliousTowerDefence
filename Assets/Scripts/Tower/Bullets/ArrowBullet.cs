using UnityEngine;

public class ArrowBullet : Bullet
{
    [SerializeField] private GameObject _bulletView;

    public override void ApplySpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletAchieveTarget()
    {
        _bulletView.SetActive(false);

        ApplyDamage();
        ReturnToPool();
    }

    public override void BulletReadyToFly()
    {
        _bulletView.SetActive(true);
    }
}
