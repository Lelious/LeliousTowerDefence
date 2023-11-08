using System.Collections.Generic;
using UnityEngine;

public interface IEffectable 
{
    public void ApplyEffect(IEffect effect);
    public void RemoveEffect(IEffect effect);
    public void RefreshEffectValues();
    public List<IEffect> GetEffects();
    public Transform GetOrigin();
    public void TickAction();
}
