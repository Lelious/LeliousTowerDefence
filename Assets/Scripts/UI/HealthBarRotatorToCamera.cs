using UnityEngine;

public class HealthBarRotatorToCamera : MonoBehaviour
{
    [SerializeField] private GameObject _quad;
    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (_quad.activeInHierarchy)
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
