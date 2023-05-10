using DG.Tweening;
using UnityEngine;

public class ShurikenRotator : MonoBehaviour
{
    private Quaternion _startRotation;
    private float _angle;

    private void Awake()
    {
        _startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        _angle -= 20f;

        Quaternion rotationY = Quaternion.AngleAxis(_angle, Vector3.up);       

        transform.rotation = _startRotation * rotationY;
    }
}
