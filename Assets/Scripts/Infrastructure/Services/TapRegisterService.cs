using Infrastructure.Services.Input;
using System;
using UnityEngine;
using Zenject;

public class TapRegisterService : IInputService
{
    public static Action OnEmptyTapRegistered;

    private SelectedFrame _selectedFrame;
    private ITouchable _touchedObj;
    private int _layerMask = 1 << 10;
    private bool _canTouch;
    private bool _canRegisterWorldTap = true;

    [Inject]
    private void Construct(SelectedFrame selectedFrame)
    {
        _layerMask = ~_layerMask;
        _selectedFrame = selectedFrame;
    }

    public void RegisterWorldTap(Vector3 mousePosition)
    {
        if (_canRegisterWorldTap)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(Camera.main.transform.position, ray.direction, out var hit, 100f, _layerMask))
            {
                if (hit.collider.TryGetComponent(out ITouchable touched))
                {
                    if (_touchedObj != touched)
                    {
                        if (_touchedObj is Component oldComp && oldComp != null)
                        {
                            _touchedObj.Untouch();
                        }
                    }
                    _touchedObj = touched;
                    _touchedObj.Touch(hit.point);
                }
                else
                {
                    ClearTouch();
                }
            }
            else
            {
                ClearTouch();
            }
        }
        _canRegisterWorldTap = true;
    }

    private void ClearTouch()
    {
        if (_touchedObj is Component oldComp && oldComp != null)
        {
            _touchedObj.Untouch();
        }
        _touchedObj = null;
        _selectedFrame.DisableFrame();
        OnEmptyTapRegistered?.Invoke();
    }

    public Vector3 GetMovementDirection(float z)
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
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
