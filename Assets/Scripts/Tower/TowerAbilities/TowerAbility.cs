using System;
using UnityEngine;

[Serializable]
public class TowerAbility
{
    public bool AttackModifier;
    public AppliedTarget AppliedTarget;
    public DamageSource DamageSource;
    public Sprite Icon;
    public float Chance;
    public float DamageMultiplier;
    public EffectData Data;
    public int AttacksToTrigger;
    public float MaxDistance;
}
public enum AppliedTarget
{
    Enemy,
    Tower
}
