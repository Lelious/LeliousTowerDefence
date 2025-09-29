using BezierSolution;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class JarEffectService : MonoBehaviour
{
    [SerializeField] private BezierWalkerWithTime _walker;
    [SerializeField] private List<BezierSpline> _pathsList = new();

    private bool _canEmit = true;

    private void Awake()
    {
        _walker.enabled = false;
        _walker.gameObject.SetActive(false);
        _walker.transform.position = transform.position;
        _walker.onPathCompleted.AddListener(delegate { OnPathSucessfulyReached();});
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EmitWalker();
        }
    }

    [ContextMenu("EmitWalker")]
    public void EmitWalker()
    {
        if(_canEmit)
        {
            _canEmit = !_canEmit;
            var path = _pathsList[Random.Range(0, _pathsList.Count)];
            _walker.NormalizedT = 0f;
            _walker.gameObject.SetActive(true);
            _walker.spline = path;
            _walker.enabled = true;
        }
    }

    private async UniTaskVoid OnPathSucessfulyReached()
    {
        await UniTask.WaitForSeconds(1f);
        _walker.enabled = false;
        _walker.gameObject.SetActive(false);
        _walker.transform.position = transform.position;
        _canEmit = true;
    }
}
