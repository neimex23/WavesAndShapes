using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento

    private PlayerControls controls;
    private Vector2 movement;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        Vector3 move = new Vector3(movement.x, movement.y, 0f);

        // Movimiento
        transform.position += move * speed * Time.deltaTime;

        // Rotación instantánea
        if (move.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public Vector2 GetMovementDirection()
    {
        return movement.normalized;
    }
}
