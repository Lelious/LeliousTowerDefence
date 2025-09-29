using BezierSolution;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class GameMap : MonoBehaviour
{
    [SerializeField] private MapCameraLimits _limits;
    [SerializeField] private Material _gameMaterial;
    [SerializeField] private Material _fogMaterial;
    [SerializeField] private Material _treeMaterial;
    [SerializeField] private BezierSpline _path;
    [SerializeField] private List<MeshRenderer> _treesList;


    public MapCameraLimits GetCameraLimits() => _limits;
    public BezierSpline GetPath() => _path; 
    public async UniTask SetColorScheme(GameMapColorScheme scheme)
    {
        Debug.Log("Set Colors");

        _gameMaterial.SetTexture("_Mask", scheme.MaskTexture);
        _gameMaterial.SetTexture("_Tex1", scheme.BuildPlaneTexture);
        _gameMaterial.SetTexture("_Tex2", scheme.CliffTexture);
        _gameMaterial.SetTexture("_Tex3", scheme.RoadTexture);
        _gameMaterial.SetTexture("_TexEmission", scheme.IsGlowingFloor ? scheme.FloorEmissionTexture : null);
        _gameMaterial.SetColor("_EmissionColorH", scheme.InnerFloorGlow);
        _gameMaterial.SetColor("_EmissionColorL", scheme.OuterFloorGlow);

        _fogMaterial.SetColor("_BaseColor", scheme.FogColor);

        var block = new MaterialPropertyBlock();

        foreach (var item in _treesList)
        {
            item.GetPropertyBlock(block);
            block.SetColor("_Color", GetRandomColor(scheme.TreeColor1, scheme.TreeColor2));
            item.SetPropertyBlock(block);
        }
        _treeMaterial.SetInt("_RenderLeaf", scheme.TreeLeaf == true ? 1 : 0);
    }

    private Color GetRandomColor(Color color1, Color color2)
    {
        return new Color(Random.Range(color1.r, color2.r), Random.Range(color1.g, color2.g), Random.Range(color1.b, color2.b));
    }
}