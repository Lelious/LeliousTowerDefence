using UnityEngine;
using DG.Tweening;
using UniRx;

public class WizardIK : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _pointToIK;

    private float _ikWeight;
    private bool _activeIK;
    private Tween _ikTween;

    void OnAnimatorIK()
    {
        if (_animator)
        {
            if (_activeIK)
            {
                if (_pointToIK != null)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _ikWeight);
                    _animator.SetIKPosition(AvatarIKGoal.LeftHand, _pointToIK.position);
                }
            }
        }
    }

    [ContextMenu("SetUnsetIK")]
    public void SetIKWeight()
    {
        _ikTween.Complete();
        _activeIK = !_activeIK;
        _ikTween = DOTween.To(() => _ikWeight, x => _ikWeight = x, _activeIK == true ? 1f : 0f, 10f);
    }
}
