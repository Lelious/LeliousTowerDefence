using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/Abilities/Effect", order = 1)]
public class EffectData : ScriptableObject
{
    public string EffectName;
    public Sprite EffectImage;
    public EffectType EffectType;
    public float EffectDuration;
    public int Damage;
    public DamageSource DamageSource;
    public float PercentageValue;
    public bool IsPeriodical;
    public float TickTime;
    [TextArea]
    public string Description;
}

