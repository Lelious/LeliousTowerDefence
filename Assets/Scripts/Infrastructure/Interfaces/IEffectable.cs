using UnityEngine;
using UniRx;

public interface IEffectable 
{
    public void ApplyEffect(IEffect effect);
    public void RemoveEffect(IEffect effect);
    public void RefreshEffectValues(IEffect effect);
    public void RemoveAllEffects();
    public ReactiveCollection<IEffect> GetEffects();
    public IEffect GetEffect(EffectType type);
    public Transform GetOrigin();
    public virtual void SetOnBuffProcessState(bool state) { }
    public bool GetProcessStatus();
    public virtual void TickAction() { }
}
