using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    public float panSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float smooth = 10f;
    public float minZoom = 3.0f;
    public float maxZoom = 25f;
    public float keyboardPanSpeed = 1.05f;

    private Camera camera;
    private Vector3 targetPos;
    private float targetZoom;
    private Vector3 lastMouse;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        targetPos = transform.position;
        targetZoom = camera.orthographicSize;
    }

    void Update()
    {
        HandleMousePan();
        HandleKeyboardPan();

        targetZoom -= Input.mouseScrollDelta.y * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * smooth); 
    }


    void HandleMousePan()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMouse;
            targetPos -= new Vector3(delta.x, delta.y, 0f)
                * panSpeed
                * camera.orthographicSize
                / Screen.height;
            lastMouse = Input.mousePosition;
        }
    }

    void HandleKeyboardPan()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A / D
        float v = Input.GetAxisRaw("Vertical");   // W / S

        if (h == 0 && v == 0) return;

        Vector3 direction = new Vector3(h, v, 0f).normalized;

        // Scale speed by zoom so it feels consistent
        float speed = keyboardPanSpeed + camera.orthographicSize;

        targetPos += direction * speed * Time.deltaTime;
    }

}
