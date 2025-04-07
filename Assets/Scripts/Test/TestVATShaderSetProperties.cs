using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVATShaderSetProperties : MonoBehaviour
{
    [SerializeField] private Color _color;
    [Range(0, 1)]
    [SerializeField] private float _speed = 1.0f;
    [Range(0, 1)]
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private MeshRenderer _renderer;

    private MaterialPropertyBlock _block;
    private void Start()
    {
        _block = new MaterialPropertyBlock();
    }

    [ContextMenu("SetColor")]
    public void SetColor()
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetVector("_AdittionalColor", _color);
        _renderer.SetPropertyBlock(_block);
    }

     [ContextMenu("SetSpeed")]
    public void SetSpeed()
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat("_AnimationSpeed", _speed);
        _renderer.SetPropertyBlock(_block);
    }

    [ContextMenu("SetOffset")]
    public void SetOffset()
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat("_AnimationOffset", _offset);
        _renderer.SetPropertyBlock(_block);
    }
}
