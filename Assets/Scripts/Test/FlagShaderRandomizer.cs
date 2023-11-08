using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagShaderRandomizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    private MaterialPropertyBlock _block;

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat("_Rand", Random.Range(0f, 40f));
        _renderer.SetPropertyBlock(_block);
    }
}
