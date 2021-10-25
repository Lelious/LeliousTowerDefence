using UnityEngine.EventSystems;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    Camera camera;
    private void Awake()
    {
        camera = Camera.main;
    }
    private void Update()
    {
            
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("1");
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))

        {

            if (eventData.delta.x > 0) Debug.Log("Right");

            else Debug.Log("Left");

            camera.transform.position += new Vector3(eventData.delta.x, 0, 0);

        }

        else

        {

            if (eventData.delta.y > 0) Debug.Log("Up");

            else Debug.Log("Down");

            camera.transform.position += new Vector3(0, 0, eventData.delta.y);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("2");
    }
}
