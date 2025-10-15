using BezierSolution;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class JarEffectService : MonoBehaviour
{
    [SerializeField] private BezierWalkerWithTime _walker;
    [SerializeField] private List<BezierSpline> _rightPaths = new();
    [SerializeField] private List<BezierSpline> _leftPaths = new();

    private void Awake()
    {
        _walker.enabled = false;
        _walker.gameObject.SetActive(false);
        _walker.transform.position = transform.position;
        _walker.onPathCompleted.AddListener(delegate { OnPathSucessfulyReached();});
    }

    public BezierWalkerWithTime GetEffectWalker() => _walker;

    [ContextMenu("EmitWalker")]
    public void EmitWalker(int side)
    {
        var path = side == 0 ? _leftPaths[Random.Range(0, _leftPaths.Count)] : _rightPaths[Random.Range(0, _rightPaths.Count)];
        _walker.NormalizedT = 0f;
        _walker.gameObject.SetActive(true);
        _walker.spline = path;
        _walker.enabled = true;
    }

    private async UniTaskVoid OnPathSucessfulyReached()
    {
        await UniTask.WaitForSeconds(1f);
        _walker.enabled = false;
        _walker.NormalizedT = 0f;
        _walker.gameObject.SetActive(false);
        _walker.transform.position = transform.position;
    }
}
