public abstract class Effect : IEffect
{
    protected IEffectable _effectable;
    protected float _duration;
    protected float _tick;
    protected float _currentDuration;
    protected float _percentage;
    protected float _currentTick;
    protected bool _periodical;
    protected EffectType _type;

    public float GetPercentage() => _percentage;
    public EffectType GetEffectType() => _type;
    public float GetDuration() => _duration;
    public bool IsTickable() => _periodical;

    public Effect(EffectType type, IEffectable effectable, float percentValue, float duration, bool periodical = false, float tick = 0f)
    {
        _effectable = effectable;
        _duration = duration;
        _percentage = percentValue;
        _periodical = periodical;
        _tick = tick;
        _currentDuration = duration;
        _currentTick = tick;
        _type = type;
    }

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
}
