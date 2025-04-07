using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class BuffService : ITickable
{
    private List<IEffectable> _effectableList = new();

    public void ApplyEffect(IEffectable effectable, IEffect effect)
    {
        ApplyBuff(effectable, effect);
    }

    public void RemoveEffectableFromList(IEffectable effectable)
    {
        _effectableList.Remove(effectable);
    }

    public void Tick()
    {
        if (_effectableList.Count > 0)
        {
            for (int i = 0; i < _effectableList.Count; i++)
            {
                var effects = _effectableList[i].GetEffects();

                for (int j = 0; j < effects.Count; j++)
                {
                    var endOfEffect = effects[j].Tick(Time.deltaTime);

                    if (endOfEffect)
                    {
                        _effectableList[i].RemoveEffect(effects[j]);
                    }
                }

                if (effects.Count == 0)
                {
                    RemoveEffectableFromList(_effectableList[i]);
                }
            }
        }
    }

    private void ApplyBuff(IEffectable effectable, IEffect effect)
    {
        IEffectable currentEffectable = null;

        for (int i = 0; i < _effectableList.Count; i++)
        {
            if (_effectableList[i].Equals(effectable))
            {
                currentEffectable = _effectableList[i];
                break;
            }
        }

        if (currentEffectable == null)
        {
            _effectableList.Add(effectable);
        }

        var sameEffect = effectable.GetEffect(effect.GetEffectType());

        if (sameEffect == null)
        {
            effectable.ApplyEffect(effect);
        }
        else
        {
            sameEffect.RefreshEffect(effect);
        }
    }
}
