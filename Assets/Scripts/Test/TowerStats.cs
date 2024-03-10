using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class TowerStats : StatData
{
    public BulletType BulletType;
    public Bullet BulletPrefab;
    public GameObject ImpactOnHit;
    public int UpgradeNumber;
    public float BulletYCurvature;
    public float ProjectileSpeed;
    public float ProjectileSpeedIncreacement;
    public bool OnTarget;
    public float ExplosionRadius;
    public float AttackRadius;
    public int TargetsCount;
    public int RicochetteCount;
    public bool IsUpgradesAvailable;
    public List<TowerAbility> Abilities = new();

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
        BulletType = data.BulletType;
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
        BulletYCurvature = data.FlyCurvature;
        ImpactOnHit = data.ImpactOnHit;
        ExplosionRadius = data.ExplosionRadius;
        ProjectileSpeedIncreacement = data.ProjectileSpeedIncreacement;
        OnTarget = data.OnTarget;
        Abilities = data.Abilities;
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
