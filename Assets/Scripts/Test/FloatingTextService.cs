using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class FloatingTextService : MonoBehaviour
{
    [SerializeField] private Transform _trackedObject;
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

    public void AddFloatingText(string value, Vector3 position, Color color)
    {
        var text = _floatingTextPool.Dequeue();

        if (text == null)        
            text = CreateFloatingText();       

        text.Lifetime = _lifeTime;
        text.CurrentVector = position;
        text.Text.text = value;
        text.ScrollSpeed = _scrollSpeed;
        text.Text.color = color;
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

[Serializable]
public class FloatingText
{
    public TextMeshProUGUI Text;
    public Vector3 CurrentVector;
    public float OffsetX;
    public Color Color;
    public float Lifetime;
    public float ScrollSpeed;
    public GameObject Object;

    public void ProcessPosition(float delta)
    {
        Lifetime -= delta;
        CurrentVector = new Vector3(CurrentVector.x + OffsetX * 0.2f, CurrentVector.y + delta * ScrollSpeed, CurrentVector.z);
        if (Mathf.Abs(OffsetX) < delta)
        {
            OffsetX = 0;
        }
        else
        {
            OffsetX = OffsetX > 0 ? OffsetX -= delta * 0.2f : OffsetX += delta * 0.2f;
        }

        Text.rectTransform.position = Camera.main.WorldToScreenPoint(CurrentVector);
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, Lifetime);
    }
}
