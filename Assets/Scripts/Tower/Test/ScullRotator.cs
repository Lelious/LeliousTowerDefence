using UnityEngine;

public class ScullRotator : MonoBehaviour
{   
    private Quaternion _startRotation;
    private float _angle;
    private float _zOffset;
    private bool _isUpperMove = true;

    private void Awake()
    {
        _startRotation = transform.rotation;
        _zOffset = 0.003f;
    }

    private void FixedUpdate()
    {
        _angle += 0.5f;
        Quaternion rotationY = Quaternion.AngleAxis(_angle, Vector3.forward);

        if (_isUpperMove)
        {
            if (transform.localPosition.z < _zOffset)
            {
                transform.localPosition += (Vector3.forward * Time.deltaTime * 0.001f);
            }
            else
            {
                _isUpperMove = false;
            }
        }
        else
        {
            if (transform.localPosition.z > -_zOffset)
            {
                transform.localPosition -= (Vector3.forward * Time.deltaTime * 0.001f);
            }
            else
            {
                _isUpperMove = true;
            }
        }

        transform.rotation = _startRotation * rotationY;
    }
}
