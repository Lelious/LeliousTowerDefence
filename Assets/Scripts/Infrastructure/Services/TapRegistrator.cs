using Infrastructure.Services.Input;
using System;
using UnityEngine;
using Zenject;

public class TapRegistrator : IInputService
{
    public static Action OnEmptyTapRegistered;

    private SelectedFrame _selectedFrame;
    private ITouchable _touchedObj;
    private Camera _camera;
    private int _layerMask = 1 << 10;
    private bool _canTouch;
    private bool _canRegisterWorldTap = true;

    [Inject]
    private void Construct(SelectedFrame selectedFrame)
    {
        _camera = Camera.main;
        _layerMask = ~_layerMask;
        _selectedFrame = selectedFrame;
    }

   public void RegisterWorldTap(Vector3 mousePosition)
   {
        if (_canRegisterWorldTap)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(_camera.transform.position, ray.direction, out var hit, Mathf.Infinity, _layerMask))
            {
                _touchedObj?.Untouch();

                hit.collider.gameObject.TryGetComponent(out _touchedObj);

                if (_touchedObj != null)
                {
                    _touchedObj.Touch();
                }
                else
                {
                    _selectedFrame.DisableFrame();
                    OnEmptyTapRegistered?.Invoke();
                }
            }
            else
            {
                _touchedObj?.Untouch();
                _selectedFrame.DisableFrame();
                OnEmptyTapRegistered?.Invoke();
            }
        }
        _canRegisterWorldTap = true;
    }

    public Vector3 GetMovementDirection(float z)
    {
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    public void Enable() => _canTouch = true;   
    public void Disable() => _canTouch = false;
    public void RegisterUITap() => _canRegisterWorldTap = false;
    public void DisableSelectedFrame() => _selectedFrame.DisableFrame();

}
