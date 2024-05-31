using UnityEngine;

public class VisualBuff : MonoBehaviour, IPoollableObject
{
    [SerializeField] private AnimationCurve _buffScaleCurve;
    [SerializeField] private PoollableType _poolType;

    private PoolService _poolService;
    private bool _isFirstInition;

    [SerializeField] private float _duration;
    [SerializeField] private float _timer = 10f;

    public void Update()
    {
        if (_timer >= _duration) return;

        _timer += Time.deltaTime;
        transform.localScale = Vector3.one * _buffScaleCurve.Evaluate(Mathf.Clamp(_timer, _isFirstInition == true ? 0.0f : 0.5f, _duration) / _duration);
    }

    public void SetupVisualBuff(float time, bool isFirstInition)
    {
        _isFirstInition = isFirstInition;
        _duration = time;
        _timer = 0f;
    }

    public void ReturnToPool()
    {
        _poolService.AddPoollable(_poolType, this);
    }

    public void SetInnactive()
    {
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public PoollableType GetPoolableType() => _poolType;

    public void SetBulletPool(PoolService pool, bool addToPool = true)    
    {
        _poolService = pool;
        if (addToPool)
        {
            ReturnToPool();
        }
    }
}
