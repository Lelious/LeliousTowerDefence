using UnityEngine;
using Zenject;

public sealed class InputService : MonoBehaviour
{
    private TapRegisterService _tapRegisterService;
    private ParentedCamera _cameraParent;
    private Vector3 _touchStart;
    private float _sensibility = 0.1f;
    private float _groundZ = 0;
    private float _touchTime = 0.2f;
    private float _touchTimeTimer;
    private bool _canTouch = true;
    private int _touchCount = 0;

    [Inject]
    private void Construct(TapRegisterService tapRegistrator)
    {
        _tapRegisterService = tapRegistrator;
    }

    private void Awake()
    {
        _cameraParent = FindObjectOfType<ParentedCamera>();
    }

    private void Update()
    {
        if (_canTouch)
        {
            _touchCount = Input.touchCount;

            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = _tapRegisterService.GetMovementDirection(_groundZ);
                _touchTimeTimer = _touchTime;
            }

            if (Input.GetMouseButton(0))
            {
                if (_touchTimeTimer >= 0)
                    _touchTimeTimer -= Time.deltaTime;

                if (_touchCount < 2)
                {
                    Vector3 direction = _touchStart - _tapRegisterService.GetMovementDirection(_groundZ);
                    _cameraParent.MoveCamera(direction);
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

                _cameraParent.ZoomCamera(new Vector3(0, deltaMagnitudeDiff * _sensibility, 0));
            }

            if (Input.GetMouseButtonUp(0))
            {
                var currentMousePos = Input.mousePosition;
                if (_touchTimeTimer > 0)
                    _tapRegisterService.RegisterWorldTap(currentMousePos);
            }
        }       
    }
}
