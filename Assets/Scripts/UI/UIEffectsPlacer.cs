using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIEffectsPlacer : MonoBehaviour
{
    [SerializeField] private List<UIEffect> _effectsList = new List<UIEffect>();
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _positionTower, _positionEnemy;

    private System.IDisposable _disposableEntity;
    private ReactiveCollection<IEffect> _effects;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public void SetEffects(ReactiveCollection<IEffect> effects)
    {
        _disposableEntity?.Dispose();
        _effects = effects;
        _disposableEntity = _effects.ObserveCountChanged(true).Subscribe(x => RecalculateAllEffects()).AddTo(_disposable);
    }

    private void RecalculateAllEffects()
    {
        HideAllButtons();

        if (_effects == null) return;

        for (int i = 0; i < _effects.Count; i++)
        {
            _effectsList[i].SetEffect(_effects[i]);
        }
    }

    private void HideAllButtons()
    {
        for (int i = 0; i < _effectsList.Count; i++)
        {
            _effectsList[i].ClearEffect();
        }
    }
}
