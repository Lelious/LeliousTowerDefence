public abstract class Effect : IEffect
{
    protected string _effectName;
    protected IEffectable _effectable;
    protected float _duration;
    protected float _tick;
    protected float _currentDuration;
    protected float _percentage;
    protected int _damagePerTick;
    protected float _currentTick;
    protected bool _periodical;
    protected string _description;
    protected EffectType _type;
    protected DamageSource _damageSource;

    public float GetPercentage() => _percentage;
    public EffectType GetEffectType() => _type;
    public float GetDuration() => _duration;
    public bool IsTickable() => _periodical;

    public bool Tick(float delta)
    {
        _currentDuration -= delta;

        if (_periodical)
        {
            _currentTick -= delta;

            if (_currentTick <= 0)
            {
                _effectable.TickAction();
                _currentTick = _tick;
            }
        }

        if (_currentDuration <= 0) return true;

        else return false;
    }

    public void RefreshEffect(IEffect effect)
    {
        _currentDuration = effect.GetDuration();
        _percentage = effect.GetPercentage();
        _periodical = effect.IsTickable();
        _effectable.RefreshEffectValues();
    }

    public DamageSource GetDamageSource() => _damageSource;
    public int GetDamage() => _damagePerTick;
}
