using BezierSolution;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    [SerializeField] private Animator _wizardAnim;
    [SerializeField] private BezierWalkerWithSpeed _wizardWalker;
    [SerializeField] private JarEffectService _effectService;
    [SerializeField] private BezierSpline _leftIn;
    [SerializeField] private BezierSpline _leftOut;
    [SerializeField] private GameObject _bottle;
    [SerializeField] private WizardPath _path;

    private int _startAnim = Animator.StringToHash("StartMixJar");
    private int _knockAnim = Animator.StringToHash("KnockBack");
    private int _walkAnim = Animator.StringToHash("Walk");
    private int _idleAnim = Animator.StringToHash("Idle");
    private int _endkAnim = Animator.StringToHash("EndMixJar");
    private int _grabkAnim = Animator.StringToHash("GrabPotion");
    private int _fillkAnim = Animator.StringToHash("FillPotion");

    private int _currentAnim;
    private bool _in;
    
    private async void Start()
    {
        _wizardWalker.enabled = false;
        _wizardWalker.onPathCompleted.AddListener(delegate { OnWizardPathReached(); });
        _currentAnim = _startAnim;
        await UniTask.WaitForSeconds(2f);
        SetNextAnimation(_startAnim);
        //RunWizardLogic();
    }

    [ContextMenu("Walk")]
    public async UniTaskVoid WalkToBookStand()
    {
        SetNextAnimation(_currentAnim == _startAnim ? _endkAnim : _idleAnim);
        await UniTask.WaitForSeconds(1f);
        SetNextAnimation(_walkAnim);
        _wizardWalker.spline = _path.GetNextPath();
        _wizardWalker.enabled = true;
    }

    private async UniTaskVoid OnWizardPathReached()
    {
        var path = _path.GetCurrentPath();
        Debug.Log(path.ToString());
        await UniTask.WaitForSeconds(0.2f);

        SetNextAnimation(path.Equals(CurrentWizardPath.LeftIn) || path.Equals(CurrentWizardPath.RightIn) ? _grabkAnim : _fillkAnim);
        await UniTask.WaitForSeconds(1.5f);

        if(path.Equals(CurrentWizardPath.LeftIn) || path.Equals(CurrentWizardPath.RightIn))
        {
            _bottle.SetActive(true);
        }

        await UniTask.WaitForSeconds(1.5f);

        if(path.Equals(CurrentWizardPath.LeftOut) || path.Equals(CurrentWizardPath.RightOut))
        {
            _bottle.SetActive(false);
        }

        SetNextAnimation(path.Equals(CurrentWizardPath.LeftIn) || path.Equals(CurrentWizardPath.RightIn) ? _idleAnim : _startAnim);
        _wizardWalker.enabled = false;
        _wizardWalker.NormalizedT = 0f;

        if(path.Equals(CurrentWizardPath.LeftIn) || path.Equals(CurrentWizardPath.RightIn))
        {
            WalkToBookStand();
        }
    }

    private void SetNextAnimation(int animationId)
    {
        _wizardAnim.SetBool(_currentAnim, false);
        _currentAnim = animationId;
        _wizardAnim.SetBool(_currentAnim, true);
    }

    private async UniTaskVoid RunWizardLogic()
    {
        await UniTask.WaitForSeconds(2);
        _wizardAnim.SetTrigger(_startAnim);

        for (int i = 0; i < 10; i++)
        {
            await UniTask.WaitForSeconds(Random.Range(4, 8));
            _effectService.EmitWalker();
            _wizardAnim.SetTrigger(_knockAnim);
        }

        _wizardAnim.SetTrigger(_endkAnim);
    }
}
