using UnityEngine;

public class CanvasRotatorToCamera : MonoBehaviour
{
    [SerializeField] private GameObject _slider;
    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (_slider.activeSelf)
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
