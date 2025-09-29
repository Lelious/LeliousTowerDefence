using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private ProgressPathData _pathData;
    [SerializeField] private MeshRenderer _pathMesh;
    [SerializeField] private List<MiniMapPoint> _pathPoints = new();
    [SerializeField] private GameObject _pathPointEffect;
    [SerializeField] private MapCameraLimits _cameraLimits;

    private TopMenuInformator _topMenuInformator;
    private Tween _pathMovingTween;
    private int _currentPathNum = 1;
    private MaterialPropertyBlock _block;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            CompletePath();
        if (Input.GetKeyDown(KeyCode.S))
            ClearPath();
    }

    public MapCameraLimits GetCameraLimits() => _cameraLimits;

    public void InitializePath(MiniMapPointInfoField miniMapInfoField)
    {
        _block = new MaterialPropertyBlock();
        _currentPathNum = PlayerPrefs.GetInt("PlayerProgress", 1);

        foreach (var item in _pathPoints)
        {
            item.InitializePoint(miniMapInfoField);
        }

        InitializeProgress();
    }

    public void CompletePath()
    {
        _pathPointEffect.SetActive(false);

        var point = _pathPoints[Mathf.Clamp(_currentPathNum, 1, _pathPoints.Count) - 1];
        point.GetMeshRenderer().GetPropertyBlock(_block);
        _block.SetFloat("_Progress", 1.2f);
        point.GetMeshRenderer().SetPropertyBlock(_block);

        _pathPointEffect.transform.position = point.transform.position;
        _pathPointEffect.SetActive(true);

        _pathMovingTween.Kill();

        var startValue = _pathData.PathDataValue[Mathf.Clamp(_currentPathNum, 1, _pathData.PathDataValue.Count - 1)];
        var endValue = _pathData.PathDataValue[Mathf.Clamp(++_currentPathNum, 1, _pathData.PathDataValue.Count - 1)];
        _pathMesh.GetPropertyBlock(_block);

        _pathMovingTween = DOTween.To(() => startValue, x => startValue = x, endValue, 2f).OnUpdate(() => 
        { 
            _block.SetFloat("_Progress", startValue);
            _pathMesh.SetPropertyBlock(_block);
        }).SetEase(Ease.InOutCubic);

        PlayerPrefs.SetInt("PlayerProgress", _currentPathNum);
    }

    public void ClearPath()
    {
        _pathMovingTween.Kill();
        _currentPathNum = 1;
        _pathMesh.GetPropertyBlock(_block);
        _block.SetFloat("_Progress", _pathData.PathDataValue[_currentPathNum]);
        _pathMesh.SetPropertyBlock(_block);

        foreach (var item in _pathPoints)
        {
            var renderer = item.GetMeshRenderer();
            renderer.GetPropertyBlock(_block);
            _block.SetFloat("_Progress", 0f);
            renderer.SetPropertyBlock(_block);
        }
        PlayerPrefs.SetInt("PlayerProgress", _currentPathNum);
    }

    private void InitializeProgress()
    {
        if(_currentPathNum > 1)
        {
            _block = new MaterialPropertyBlock();

            for (int i = 0; i < _currentPathNum - 1; i++)
            {
                var point = _pathPoints[i % _pathPoints.Count];
                var renderer = point.GetMeshRenderer();
                renderer.GetPropertyBlock(_block);
                _block.SetFloat("_Progress", 1.2f);
                renderer.SetPropertyBlock(_block);
            }          
        }
        _pathMesh.GetPropertyBlock(_block);
        _block.SetFloat("_Progress", _pathData.PathDataValue[Mathf.Clamp(_currentPathNum, 1, _pathData.PathDataValue.Count - 1)]);

        _pathMesh.SetPropertyBlock(_block);
    }
}
