using UnityEngine;
using DG.Tweening;

public class BuffBullet : Bullet
{
    [SerializeField] private float _flySpeed = 1f;
    [SerializeField] private float _flyCurve = 0.1f;
    [SerializeField] private ParticleSystem _hitImpact;
    [SerializeField] private float _appearTime = 0.5f;

    public VisualBuff buff;
    private Tween _scaleTween;

    private void Awake()
    {
        _flyingSpeed = _flySpeed;
        _curvature = _flyCurve;
        _impactOnHit = _hitImpact.gameObject;
        _hitImpact.transform.SetParent(null);
    }

    public override void BulletAchieveTarget()
    {
        _effectable.SetOnBuffProcessState(false);
        IEffect newEffect = new Buff(_effectable, _effectData);
        var visual = _poolService.GetObjectFromPool(_effectData.PoollableType);

        if (visual == null)
        {
            visual = Instantiate(_effectData.VisualBuff);
            visual.SetBulletPool(_poolService, false);
        }

        buff = visual as VisualBuff;
        newEffect.SetVisual(buff);
        _buffService.ApplyEffect(_effectable, newEffect);

        CreateHitImpact(Vector3.up);
        ReturnToPool();
    }

    private void ReInitProjectile()
    {
        _scaleTween.Complete();
        transform.localScale = Vector3.zero;
        float time = 0f;

        _scaleTween = DOTween.To(() => time, x => time = x, 1f, _appearTime).OnUpdate(() => 
        {
            transform.localScale = Vector3.one * time;
        });
    }

    private void OnEnable()
    {
        ReInitProjectile();
    }
}
