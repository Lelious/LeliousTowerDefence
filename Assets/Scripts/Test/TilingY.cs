using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingY : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float _speed;

    private void Update()
    {
        _renderer.materials[0].mainTextureOffset = new Vector2(0, _speed * Time.time);
    }
}
