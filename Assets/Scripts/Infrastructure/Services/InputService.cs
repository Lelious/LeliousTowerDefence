using Infrastructure.Services.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : MonoBehaviour, IInputService
{
    private Vector3 _touchStart, _touchSecond;
    private ParentedCamera _cameraParent;
    private Camera _camera;
    private float groundZ = 0;
    private float _startingDistance;
    private int _touchCount = 1;
    private float _sensibility = 0.3f;

    private void Awake()
    {
        _cameraParent = FindObjectOfType<ParentedCamera>();
        _camera = Camera.main;
    }
    void Update()
    {
        _touchCount = Input.touchCount;

        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = GetWorldPosition(groundZ);
        }

        if (Input.GetMouseButton(0))
        {
            if (_touchCount < 2)
            {
                Vector3 direction = _touchStart - GetWorldPosition(groundZ);
                _cameraParent.transform.position += direction;
            }                     
        }

        if (_touchCount >= 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touchOnePrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            _cameraParent.transform.position += new Vector3(_cameraParent.transform.position.x, _cameraParent.transform.position.y + deltaMagnitudeDiff * _sensibility, _cameraParent.transform.position.z);
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    private float GetDistanceBetweenTouches()
    {
        return Vector3.Distance(_touchStart, _touchSecond);
    }

    public Vector3 GetMovementDirection()
    {
        throw new System.NotImplementedException();
    }

    public void Enable()
    {
        throw new System.NotImplementedException();
    }

    public void Disable()
    {
        throw new System.NotImplementedException();
    }
}
