using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class FloatingTextService : MonoBehaviour
{
    [SerializeField] private FloatingTextColorPalette _palette;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _horizontalOffset;

    private bool _initialized;
    private Queue<FloatingText> _floatingTextPool = new Queue<FloatingText>();
    private List<FloatingText> _activePool = new List<FloatingText>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            _floatingTextPool.Enqueue(CreateFloatingText());
        }
        _initialized = true;
    }

    public void AddFloatingText(string value, Vector3 position, DamageSource source)
    {
        var text = _floatingTextPool.Dequeue();

        if (text == null)        
            text = CreateFloatingText();       

        text.Lifetime = _lifeTime;
        text.CurrentVector = position;       
        text.ScrollSpeed = _scrollSpeed;
        text.Text.color = _palette.GetColor(source);       

        switch (source)
        {
            case DamageSource.Normal:
                text.Object.transform.localScale = Vector3.one;
                break;
            case DamageSource.Critical:
                text.Object.transform.localScale = Vector3.one * 1.2f;
                value += "!";
                break;
            default:
                text.Object.transform.localScale = Vector3.one * 0.8f;
                break;
        };
        text.Text.text = value;
        float side = UnityEngine.Random.Range(-_horizontalOffset, _horizontalOffset);
        text.OffsetX = side < 0 ? -_horizontalOffset : _horizontalOffset;
        text.ProcessPosition(0f);
        text.Object.SetActive(true);

        _activePool.Add(text);
    }

    private FloatingText CreateFloatingText()
    {
        var text = Instantiate(_textPrefab, transform);
        text.gameObject.SetActive(false);
        var floatingText = new FloatingText();
        floatingText.Text = text;
        floatingText.ScrollSpeed = _scrollSpeed;
        floatingText.Object = text.gameObject;
        
        return floatingText;
    }

    private void LateUpdate()
    {
        if (!_initialized) return;
        if (_activePool.Count == 0) return;

        for (int i = 0; i < _activePool.Count; i++)
        {
            _activePool[i].ProcessPosition(Time.deltaTime);

            if (_activePool[i].Lifetime <= 0)
            {
                _floatingTextPool.Enqueue(_activePool[i]);
                _activePool[i].Object.SetActive(false);
                _activePool.Remove(_activePool[i]);
            }
        }      

    }
}
