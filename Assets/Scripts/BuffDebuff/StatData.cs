using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class StatData
{
    public Sprite MainImage;
    public string Name;
    public int MinimalDamage;
    public int MaximumDamage;
    public float AttackSpeed;
    public int MaxHealth;
    public Dictionary<StatType, FloatReactiveProperty> UpgradableStats = new();

    public abstract void UpgradeStat(StatType type, float value);
    public abstract void InitializeInfoContainer(ReactiveCollection<IEffect> currentEffects);
    public abstract GamePannelUdaterInfoContainer GetContainer();
}
