using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.25f;

    private PlayerController playerController;
    private Vector2 dashDirection;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float cooldownTimer = 0f;

    private PlayerControls controls;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        controls = new PlayerControls();
        controls.Player.Dash.performed += _ => TryDash();
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
        cooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0f)
                isDashing = false;
        }
    }

    private void TryDash()
    {
        if (cooldownTimer <= 0f)
        {
            dashDirection = playerController.GetMovementDirection();
            if (dashDirection == Vector2.zero)
                dashDirection = Vector2.up;

            isDashing = true;
            dashTimeLeft = dashDuration;
            cooldownTimer = dashCooldown;
        }
    }

    public bool IsDashing() => isDashing;
}
