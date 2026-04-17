using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    public float panSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float smooth = 10f;
    public float minZoom = 3.0f;
    public float maxZoom = 25f;

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
        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMouse;
            targetPos -= new Vector3(delta.x, delta.y, 0f) * panSpeed * camera.orthographicSize / Screen.height;
            lastMouse = Input.mousePosition;
        }

        targetZoom -= Input.mouseScrollDelta.y * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * smooth); 
    }
}
