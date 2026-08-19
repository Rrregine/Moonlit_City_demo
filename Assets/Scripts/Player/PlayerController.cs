using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Vector2 facingDirection = Vector2.left;

    public Vector2 FacingDirection => facingDirection;

    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        movement = controls.Gameplay.Move.ReadValue<Vector2>();

        if (movement.x != 0)
        {
            facingDirection = new Vector2(
                Mathf.Sign(movement.x),
                0
            );
            //Debug.Log($"Facing Direction: {facingDirection}");
        }

        movement = movement.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

}
