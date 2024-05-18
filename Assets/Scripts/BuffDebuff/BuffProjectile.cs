using UnityEngine;
using DG.Tweening;

public class BuffProjectile : MonoBehaviour
{
    [SerializeField] private MeshRenderer _glowOrbRenderer;
    [SerializeField] private float _timeToChangeScale = 1f;

    private MaterialPropertyBlock _block;
    private Tween _scaleTween;

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
        _glowOrbRenderer.GetPropertyBlock(_block);
    }

    public void SetScale(Vector3 scale, bool instant = false)
    {
        _scaleTween.Complete();

        var timeToScale = instant ? 0f : _timeToChangeScale;
        var timer = 0f;
        _scaleTween = DOTween.To(() => timer, x => timer = x, 1f, timeToScale).OnUpdate(() => 
        {
            transform.localScale = scale * timer;
            _block.SetVector("_Scale", scale * timer);
            _glowOrbRenderer.SetPropertyBlock(_block);
        }).SetEase(Ease.Linear);
    }
}
