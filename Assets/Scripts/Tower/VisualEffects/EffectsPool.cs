using System.Collections.Generic;
using UnityEngine;

public class EffectsPool : MonoBehaviour
{
    private List<IPoollableEffect> _effectsPool = new List<IPoollableEffect>();
    private GameObject _effectPrefab;

    public void InitializePool(GameObject effectPrefab, int poolSize)
    {
        _effectPrefab = effectPrefab;

        for (int i = 0; i < poolSize; i++)
            SpawnBullet();
    }

    public IPoollableEffect GetBulletFromPool()
    {
        var effect = _effectsPool.Find(x => x.IsFree() == true);

        if (effect != null)
        {
            _effectsPool.Remove(effect);
            return effect;
        }
        else
        {
            return SpawnOverPoolEffect();
        }
    }

    public void Return(IPoollableEffect effect)
    {
        effect.SetInnactive();
        _effectsPool.Add(effect);
    }

    private void SpawnBullet()
    {
        var newEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity).GetComponent<IPoollableEffect>();
        newEffect.SetPool(this);
        newEffect.SetInnactive();
        _effectsPool.Add(newEffect);
    }

    private IPoollableEffect SpawnOverPoolEffect()
    {
        var newEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity).GetComponent<IPoollableEffect>();
        newEffect.SetPool(this);
        newEffect.SetInnactive();
        return newEffect;
    }
}
