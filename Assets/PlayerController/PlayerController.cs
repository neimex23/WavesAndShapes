using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f; 

    private PlayerControls controls;
    private Vector2 movement;
    private PlayerDash playerDash;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;
        playerDash = GetComponent<PlayerDash>();
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
        transform.position += move * playerSpeed * Time.deltaTime;

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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBall") && !playerDash.IsDashing())
        {
            var hitFeedback = GetComponent<PlayerHitFeedback>();
            var healthControl = GetComponent<PlayerHealth>();
            if (!hitFeedback.IsInvincible())
            {
                hitFeedback.TriggerHit(collision.transform.position);
                healthControl.TakeDamage(1);
            }
            Destroy(collision.gameObject);
            Debug.Log("Colisión con enemigo detectada");
        }
    }
}
