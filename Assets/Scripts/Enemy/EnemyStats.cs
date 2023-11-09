using UniRx;

public class EnemyStats : StatData
{
    public FloatReactiveProperty Armor = new();
    public FloatReactiveProperty Health = new();

    private GamePannelUdaterInfoContainer _container;

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

        InitializeUpgradableStats();      
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

    public override void UpgradeStat(StatType type, float value)
    {
        
    }

    private void InitializeUpgradableStats()
    {
        UpgradableStats.Add(StatType.Armor, Armor);
        UpgradableStats.Add(StatType.Health, Health);
    }
}
