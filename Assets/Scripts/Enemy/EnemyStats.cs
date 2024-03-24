using System.Collections.Generic;
using UniRx;

public class EnemyStats : StatData
{
    public FloatReactiveProperty Armor = new();
    public FloatReactiveProperty Health = new();
    public FloatReactiveProperty BonusSpeed = new(1f);
    private GamePannelUdaterInfoContainer _container;
    public float Speed;

    public override GamePannelUdaterInfoContainer GetContainer() => _container;

    public void InitializeStats(EnemyData data)
    {     
        MainImage = data.MainImage;
        Name = data.Name;
        MaxHealth = data.MaxHP;
        Armor.Value = data.Armor;
        MinimalDamage = data.MinimalDamage;
        MaximumDamage = data.MaximumDamage;
        Health.Value = MaxHealth;
        Speed = data.Speed;
        InitializeUpgradableStats();      
    }

    public override void InitializeInfoContainer(ReactiveCollection<IEffect> currentEffects)
    {
        _container = new GamePannelUdaterInfoContainer
            (
                MainImage,
                Name,
                MaxHealth,
                MinimalDamage,
                MaximumDamage,
                AttackSpeed,
                UpgradableStats,
                currentEffects
            );
    }

    public override void UpgradeStat(StatType type, float value)
    {
        switch (type)
        {      
            case StatType.BonusSpeed:
                BonusSpeed.Value += value;
                break;
        }
    }

    private void InitializeUpgradableStats()
    {
        UpgradableStats.Add(StatType.Armor, Armor);
        UpgradableStats.Add(StatType.Health, Health);
        UpgradableStats.Add(StatType.BonusSpeed, BonusSpeed);
    }
}
