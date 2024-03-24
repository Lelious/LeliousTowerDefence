using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonSpriteSwapper : MonoBehaviour
{
    [SerializeField] private float _animationPeriod = 0.5f;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _normalSprite, _highlightedSprite;
    private Coroutine _animationRoutine;

    public void StartAnimation()
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }

        _image.sprite = _normalSprite;
        _animationRoutine = StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_animationPeriod);
            _image.sprite = _highlightedSprite;
            yield return new WaitForSeconds(_animationPeriod);
            _image.sprite = _normalSprite;
        }
    }

    private void OnDisable()
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }
    }
}
