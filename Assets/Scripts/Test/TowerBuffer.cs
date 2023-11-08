using System.Collections;
using UnityEngine;
using Zenject;
using DG.Tweening;
using System.Collections.Generic;

public class TowerBuffer : MonoBehaviour
{
    [SerializeField] private EffectType _type;
    [SerializeField] private float _duration;
    [SerializeField] private float _tick;
    [SerializeField] private bool _periodical;
    [SerializeField] private float _percentage;
    [SerializeField] private float _buffRadius;
    [SerializeField] private GameObject _fireBuffOrb;
    [SerializeField] private GameObject _waterBuffOrb;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float _buffReload;
    [SerializeField] private LayerMask _mask;
    [Inject] private BuffService _buffService;

    private EffectType _emittedType;
    private List<IEffectable> _effectablesList = new();

    private void Awake()
    {
        StartCoroutine(BuffRoutine());
    }

    private IEnumerator BuffRoutine()
    {
        while (true)
        {
            _effectablesList.Clear();
            Collider[] targets = new Collider[30];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _buffRadius, targets, _mask);

            for (int i = 0; i < numColliders; i++)
            {
                targets[i].TryGetComponent<IEffectable>(out var effectable);

                if (effectable != null)
                {
                    _effectablesList.Add(effectable);
                }
            }

            if (ApplyEffectOnEffectable())
            {
                yield return new WaitForSeconds(_buffReload);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private bool ApplyEffectOnEffectable()
    {
        if (_effectablesList.Count > 0)
        {
            var effectable = _effectablesList[Random.Range(0, _effectablesList.Count)];
            GameObject buffBall = null;

            _emittedType = _type;

            switch (_type)
            {
                case EffectType.IncreaceAttackPower:
                    buffBall = Instantiate(_waterBuffOrb, transform.position, Quaternion.identity);
                    _type = EffectType.IncreaceAttackSpeed;
                    break;
                case EffectType.IncreaceAttackSpeed:
                    buffBall = Instantiate(_fireBuffOrb, transform.position, Quaternion.identity);
                    _type = EffectType.IncreaceAttackPower;
                    break;
            }

            buffBall.transform.DOJump(effectable.GetOrigin().position, 5f, 1, 2f).OnComplete(()=>
            {
                _buffService.ApplyEffect(effectable, new TestBuff(_emittedType, effectable, _percentage, _duration, false));
                Destroy(buffBall);
            });

            return true;
        }

        else return false;
    }
}
