using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.25f;

    [Header("Squash Settings")]
    public Vector3 dashScale = new Vector3(-1.5f, 1f, 1f); 
    public float scaleReturnSpeed = 10f; 

    private PlayerController playerController;
    private TrailRenderer dashtrail;
    private Vector2 dashDirection;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float cooldownTimer = 0f;

    private PlayerControls controls;
    private Vector3 originalScale;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        dashtrail = GetComponent<TrailRenderer>();
        if (dashtrail != null) dashtrail.emitting = false;

        controls = new PlayerControls();
        controls.Player.Dash.performed += _ => TryDash();

        originalScale = transform.localScale;
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
            if (dashtrail != null) dashtrail.emitting = true;
            transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, dashScale, Time.deltaTime * 15f);

            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0f)
                isDashing = false;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleReturnSpeed);

            if (dashtrail != null)
            {
                dashtrail.Clear();
                dashtrail.emitting = false;
            }
        }
    }

    private void TryDash()
    {
        if (cooldownTimer <= 0f)
        {
            dashDirection = playerController.GetMovementDirection();
            if (dashDirection == Vector2.zero)
                dashDirection = transform.up;

            isDashing = true;
            dashTimeLeft = dashDuration;
            cooldownTimer = dashCooldown;
        }
    }

    public bool IsDashing() => isDashing;
}
