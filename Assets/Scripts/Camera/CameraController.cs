using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ---------- Zoom ----------
    [Header("Zoom")]
    [SerializeField]
    private float zoomSpeed = 2f;

    [SerializeField]
    private float minZoom = 3f;

    [SerializeField]
    private float maxZoom = 10f;

    // ---------- Pan ----------
    [Header("Pan")]
    [SerializeField]
    private float panSpeed = 1f;

    private Camera cam;

    private PlayerControls controls;

    private bool isDragging = false;

    private Vector2 dragStartMouseScreenPosition;

    private Vector3 dragStartCameraPosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
    }

    // ---------- Zoom ----------
    private void HandleZoom()
    {
        if (!controls.Camera.Modifier.IsPressed())
            return;

        float scroll =
            controls.Camera.Zoom.ReadValue<float>();

        if (scroll == 0f)
            return;

        float zoomAmount =
            scroll * zoomSpeed;

        cam.orthographicSize -= zoomAmount;

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize,
            minZoom,
            maxZoom
        );
    }

    // ---------- Pan ----------
    private void HandlePan()
    {
        if (!controls.Camera.Modifier.IsPressed())
        {
            isDragging = false;
            return;
        }

        // Start dragging
        if (controls.Camera.Pan.WasPressedThisFrame())
        {
            isDragging = true;

            dragStartMouseScreenPosition =
                controls.Camera.MousePosition.ReadValue<Vector2>();

            dragStartCameraPosition =
                transform.position;

            return;
        }

        // Stop dragging
        if (controls.Camera.Pan.WasReleasedThisFrame())
        {
            isDragging = false;
            return;
        }

        if (!isDragging)
            return;

        Vector2 currentMouseScreenPosition =
            controls.Camera.MousePosition.ReadValue<Vector2>();

        Vector2 mouseDifference =
            dragStartMouseScreenPosition -
            currentMouseScreenPosition;

        float worldUnitsPerPixel =
            (cam.orthographicSize * 2f) /
            Screen.height;

        Vector3 cameraOffset =
            new Vector3(
                mouseDifference.x * worldUnitsPerPixel,
                mouseDifference.y * worldUnitsPerPixel,
                0f
            );

        transform.position =
            dragStartCameraPosition +
            cameraOffset * panSpeed;
    }
}