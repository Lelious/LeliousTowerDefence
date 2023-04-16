using UnityEngine;

public class MagicTowerCircleRotator : MonoBehaviour
{
    [SerializeField] private bool _clockwiseMove;
    [SerializeField] private float _speed = 1f;

    private Quaternion _startRotation;
    private float _angle;
    private float _angleStep;

    private void Awake()
    {
        _startRotation = transform.rotation;

        if (_clockwiseMove)     
            _angleStep = 1f;
        else      
            _angleStep = -1f;      
    }

    private void FixedUpdate()
    {
        _angle += _angleStep;

        Quaternion rotationY = Quaternion.AngleAxis(_angle * _speed, Vector3.up);

        transform.rotation = _startRotation * rotationY;
    }
}
