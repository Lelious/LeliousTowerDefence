using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using System.Collections;

public sealed class BuffService : ITickable
{
    private List<IEffectable> _effectableList = new();

    public void ApplyEffect(IEffectable effectable, IEffect effect)
    {
        var currentEffectable = _effectableList.Find(x => x == effectable);

        if (currentEffectable == null)
        {
            _effectableList.Add(effectable);
        }

        var effects = effectable.GetEffects();
        var type = effect.GetEffectType();
        var sameEffect = effects.Find(x => x.GetEffectType() == type);

        if (sameEffect == null)
        {
            effectable.ApplyEffect(effect);
        }
        else
        {
            sameEffect.RefreshEffect(effect);
        }
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
}
