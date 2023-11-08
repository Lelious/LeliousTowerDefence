using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotator : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0f, _speed, 0f);
    }
}
