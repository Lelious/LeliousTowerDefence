using Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

public class InputService : MonoBehaviour, IInputService
{   
    private GameInformationMenu _gameInformationMenu;
    private SelectedFrame _selectedFrame;
    private ParentedCamera _cameraParent;
    private Camera _camera;
    private ITouchable _touchedObj;
    private Vector3 _touchStart;
    private float _sensibility = 0.1f;
    private float _groundZ = 0;
    private float _touchTime = 0.25f;
    private float _touchTimeTimer;
    private bool _canTouch = true;
    private int _touchCount = 0;

    [Inject]
    private void Construct(GameInformationMenu gameInformationMenu, SelectedFrame selectedFrame)
    {
        _gameInformationMenu = gameInformationMenu;
        _selectedFrame = selectedFrame;
    }

    private void Awake()
    {
        _gameInformationMenu = FindObjectOfType<GameInformationMenu>();
        _cameraParent = FindObjectOfType<ParentedCamera>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_canTouch)
        {
            _touchCount = Input.touchCount;

            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = GetMovementDirection(_groundZ);
                _touchTimeTimer = _touchTime;
            }

            if (Input.GetMouseButton(0))
            {
                if (_touchTimeTimer >= 0)
                    _touchTimeTimer -= Time.deltaTime;

                if (_touchCount < 2)
                {
                    Vector3 direction = _touchStart - GetMovementDirection(_groundZ);
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

                _cameraParent.transform.position += new Vector3(0, deltaMagnitudeDiff * _sensibility, 0);
            }

            if (Input.GetMouseButtonUp(0))
            {
                var currentMousePos = Input.mousePosition;
                if (_touchTimeTimer > 0)
                    RegisterTap(currentMousePos);
            }
        }       
    }

    public ITouchable GetCurrentTouchable() => _touchedObj;
    public void Enable() => _canTouch = true;
    public void Disable() => _canTouch = false;

    public Vector3 GetMovementDirection(float z)
    {
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    private void RegisterTap(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            _touchedObj?.Untouch();

            hit.collider.gameObject.TryGetComponent(out _touchedObj);

            if (_touchedObj != null)
            {
                _touchedObj.Touch();
                _selectedFrame.EnableFrame();
                _selectedFrame.transform.position = _touchedObj.GetPosition();
            }
        }
        else
        {
            _touchedObj?.Untouch();
            _selectedFrame.DisableFrame();
            _gameInformationMenu.HideEmptyCellMenu();
            _gameInformationMenu.HideGameMenu();           
        }
    }
}
