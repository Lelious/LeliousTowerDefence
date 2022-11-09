using UnityEngine;

public class CameraFrustrumChecker : MonoBehaviour
{
    private Camera _camera;
    private CapsuleCollider _collider;
    private protected void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _camera = Camera.main;
    }

    public bool CheckForVisibility()
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), _collider.bounds);
    }
}
