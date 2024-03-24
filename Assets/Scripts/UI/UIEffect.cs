using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEffect : MonoBehaviour
{
    [SerializeField] private BottomGameMenu _gameMenu;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private Image _image;
    private EffectInfo _info;
    private IEffect _effect;

    private void Update()
    {
        if (_effect == null) return;

        SetText(string.Format("{0:f0}", _effect.GetDuration()));
    }

    private void SetText(string text) => _timer.text = text;

    public void SetEffect(IEffect effect)
    {
        _effect = effect;
        _info = _effect.GetEffectInfo();
        _image.sprite = _info.Image;
        _image.color = new Color(1, 1, 1, 1);
    }

    public void ClearEffect()
    {
        _image.sprite = null;
        _image.color = new Color(1, 1, 1, 0);
        _effect = null;
        _timer.text = "";
    }

    public void ShowInfo()
    {
        _gameMenu.ShowInfoBox(_info.Name, _info.Description);
    }
}
