using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class TowerStats : StatData
{
    public Bullet BulletPrefab;
    public int UpgradeNumber;
    public float ProjectileSpeed;
    public float ExplosionRadius;
    public float AttackRadius;
    public int TargetsCount;
    public int RicochetteCount;
    public bool IsUpgradesAvailable;

    public FloatReactiveProperty BonusAttackPower = new();
    public FloatReactiveProperty BonusAttackSpeed = new();
    public FloatReactiveProperty Health = new();

    public List<TowerData> UpgradablesList = new();

    private GamePannelUdaterInfoContainer _container;

    public override GamePannelUdaterInfoContainer GetContainer() => _container;

    public void InitializeStats(TowerData data)
    {
        Name = data.Name;
        MainImage = data.MainImage;
        BulletPrefab = data.BulletPrefab;
        AttackSpeed = data.AttackSpeed;
        MinimalDamage = data.MinimalDamage;
        MaximumDamage = data.MaximumDamage;
        MaxHealth = data.MaxHP;
        ProjectileSpeed = data.ProjectileSpeed;
        TargetsCount = data.TargetsCount;
        RicochetteCount = data.RicochetteCount;
        AttackRadius = data.AttackRadius;
        UpgradablesList = data.UpgradablesList;

        InitializeUpgradableStats();
    }

    public override void UpgradeStat(StatType type, float value)
    {
        switch (type)
        {
            case StatType.BonusAttackPower:
                BonusAttackPower.Value = value > 0 ? Mathf.FloorToInt((MinimalDamage + MaximumDamage) / 2 * value) : 0;
                break;
            case StatType.BonusAttackSpeed:
                BonusAttackSpeed.Value = AttackSpeed * value;
                break;
        }
    }

    public override void InitializeInfoContainer()
    {
        _container = new GamePannelUdaterInfoContainer
            (
                MainImage,
                Name,
                MaxHealth,
                MinimalDamage,
                MaximumDamage,
                AttackSpeed,
                UpgradableStats
            );
    }

    private void InitializeUpgradableStats()
    {
        UpgradableStats.Add(StatType.Health, Health);
        UpgradableStats.Add(StatType.BonusAttackPower, BonusAttackPower);
        UpgradableStats.Add(StatType.BonusAttackSpeed, BonusAttackSpeed);
    }     
}
