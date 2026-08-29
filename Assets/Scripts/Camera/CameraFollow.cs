using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    public Transform target;

    private PlayerControls controls;

    private void Awake()
    {
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

    private void LateUpdate()
    {
        // Stop following while Ctrl is held.
        if (controls.Camera.Modifier.IsPressed())
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}