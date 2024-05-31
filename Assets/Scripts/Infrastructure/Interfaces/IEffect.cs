using UnityEngine;

public interface IEffect
{
    public float GetDuration();
    public bool Tick(float delta);
    public void RefreshEffect(IEffect effect);
    public bool IsTickable();
    public EffectType GetEffectType();
    public PoollableType GetPoollableType();
    public DamageSource GetDamageSource();
    public float GetPercentage();
    public int GetDamage();
    public VisualBuff GetVisualBuff();
    public void SetVisual(VisualBuff visualBuff);
    public EffectInfo GetEffectInfo();
}