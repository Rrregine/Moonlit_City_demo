using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    public Transform target;

    void LateUpdate()
    {
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
