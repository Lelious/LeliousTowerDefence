using UnityEngine;
using DG.Tweening;

public class BuffBullet : Bullet
{
    [SerializeField] private float _flySpeed = 1f;
    [SerializeField] private float _flyCurve = 0.1f;
    [SerializeField] private ParticleSystem _hitImpact, _buffFx;
    [SerializeField] private float _appearTime = 0.5f;

    private GameObject _buffObj;
    private Tween _scaleTween;

    private void Awake()
    {
        _flyingSpeed = _flySpeed;
        _curvature = _flyCurve;
        _impactOnHit = _hitImpact.gameObject;
        _hitImpact.transform.SetParent(null);
        _buffObj = _buffFx.gameObject;
        _buffObj.transform.SetParent(null);
    }

    public override void BulletAchieveTarget()
    {
        _effectable.SetOnBuffProcessState(false);
        _buffService.ApplyEffect(_effectable, new Buff(_effectable, _effectData));
        var buffFx = _buffFx.main;
        var buffFxSub = _buffFx.GetComponentInChildren<ParticleSystem>().main;
        buffFx.duration = _effectData.EffectDuration;
        buffFxSub.duration = _effectData.EffectDuration;
        _buffObj.transform.position = _effectable.GetOrigin().position;
        _buffObj.SetActive(true);

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

        _impactOnHit.SetActive(false);
        _buffObj.SetActive(false);
    }
    private void OnEnable()
    {
        ReInitProjectile();
    }
}
