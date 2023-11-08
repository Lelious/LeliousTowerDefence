public interface IEffect
{
    public float GetDuration();
    public bool Tick(float delta);
    public void RefreshEffect(IEffect effect);
    public bool IsTickable();
    public EffectType GetEffectType();
    public float GetPercentage();
}