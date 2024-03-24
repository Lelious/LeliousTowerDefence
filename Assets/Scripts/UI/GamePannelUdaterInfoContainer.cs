using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class GamePannelUdaterInfoContainer
{
    public ITouchable Touchable;
    public Sprite PreviewImage { get; private set; }
    public string Name { get; private set; }
    public int MaxHealth { get; private set; }
    public float MinDamage { get; private set; }
    public float MaxDamage { get; private set; }
    public float AttackSpeed { get; private set; }
    public ReactiveCollection<IEffect> CurrentEffects { get; private set; }

    public Dictionary<StatType, FloatReactiveProperty> UpgradableStats = new Dictionary<StatType, FloatReactiveProperty>();
    public List<TowerData> UpgradesList;

    public GamePannelUdaterInfoContainer(Sprite previewImage, 
                                         string name, int maxHealth, 
                                         float minDamage, 
                                         float maxDamage, 
                                         float attackSpeed,
                                         Dictionary<StatType, FloatReactiveProperty> upgradableStats,
                                         ReactiveCollection<IEffect> effects)
    {
        PreviewImage = previewImage;
        Name = name;
        MaxHealth = maxHealth;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        AttackSpeed = attackSpeed;
        UpgradableStats = upgradableStats;
        CurrentEffects = effects;
    }
}
