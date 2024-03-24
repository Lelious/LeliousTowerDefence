using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class FloatingTextService : MonoBehaviour
{
    [SerializeField] private FloatingTextColorPalette _palette;
    [SerializeField] private TextMeshPro _textPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _horizontalOffset;
    [SerializeField] private float _initialSize = 0.04f;

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
        _floatingTextPool.TryDequeue(out var text);

        if (text == null)        
            text = CreateFloatingText();       

        text.Lifetime = _lifeTime;
        text.CurrentVector = position;       
        text.ScrollSpeed = _scrollSpeed;
        text.Text.color = _palette.GetColor(source);       

        switch (source)
        {
            case DamageSource.Normal:
                text.Object.transform.localScale = Vector3.one * _initialSize;
                break;
            case DamageSource.Critical:
                text.Object.transform.localScale = Vector3.one * 1.2f * _initialSize;
                value += "!";
                break;
            default:
                text.Object.transform.localScale = Vector3.one * 0.8f * _initialSize;
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
