using UniRx;
using UnityEngine;

public abstract class Effect : IEffect
{
    protected FloatReactiveProperty CurrentDuration = new();
    
    protected DamageSource _damageSource;
    protected IEffectable _effectable;
    protected EffectType _type;
    protected Sprite _image;

    protected string _description;
    protected int _damagePerTick;
    protected float _currentTick;
    protected string _effectName;
    protected float _percentage;
    protected bool _periodical;
    protected float _tick;

    public EffectInfo GetEffectInfo() => new EffectInfo(_effectName, _description, _image);
    public DamageSource GetDamageSource() => _damageSource;
    public float GetDuration() => CurrentDuration.Value;
    public float GetPercentage() => _percentage;
    public EffectType GetEffectType() => _type;
    public int GetDamage() => _damagePerTick;
    public bool IsTickable() => _periodical;  

    public bool Tick(float delta)
    {
        CurrentDuration.Value -= delta;

        if (_periodical)
        {
            _currentTick -= delta;

            if (_currentTick <= 0)
            {
                _effectable.TickAction();
                _currentTick = _tick;
            }
        }

        if (CurrentDuration.Value <= 0) return true;

        else return false;
    }

    public void RefreshEffect(IEffect effect)
    {
        CurrentDuration.Value = effect.GetDuration();
        _percentage = effect.GetPercentage();
        _periodical = effect.IsTickable();
        _effectable.RefreshEffectValues();
    }
}

public struct EffectInfo
{
    public string Name;
    public string Description;
    public Sprite Image;
    public EffectInfo(string name, string description, Sprite image)
    {
        Name = name;
        Description = description;
        Image = image;
    }
}
