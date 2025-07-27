using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f;
    public Vector2 currentPosition { get; private set; }

    private PlayerControls controls;
    private Vector2 movement;
    private PlayerDash playerDash;
    private PlayerParry playerParry;

    private LevelAttackManager levelAttackManager;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;

        controls.Player.DebugBall.performed += _ => SpawnBall();
        controls.Player.DebugLine.performed += _ => SpawnLine();

        playerDash = GetComponent<PlayerDash>();
        playerParry = GetComponent<PlayerParry>();
        levelAttackManager = FindFirstObjectByType<LevelAttackManager>();    
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

        currentPosition = transform.position;

        if (move.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void SpawnBall()
    {
        if (levelAttackManager != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1f, 0);
            Vector3 direction = transform.up;
            levelAttackManager.SpawnEnemyBall(spawnPosition, direction, 10f);
        }
    }

    private void SpawnLine()
    {
        if (levelAttackManager != null)
        {
            Vector2 start = transform.position;
            Vector2 end = start + (Vector2)transform.up * 5f;
            levelAttackManager.SpawnEnemyLine(start, end, EnemyLine.LineType.Block,2);
        }
    }

    public Vector2 GetMovementDirection()
    {
        return movement.normalized;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyLine"))
        {
            Debug.Log("Colisión con EnemyLine detectada");
            EnemyLine enemyLine = collision.GetComponent<EnemyLine>();

            var lineType = collision.GetComponent<EnemyLine>().lineType;
            if (lineType == EnemyLine.LineType.Damage)
            {
                ProcessHit(collision);
            }
            else if (lineType == EnemyLine.LineType.Block)
            {
                // Línea azul: hit solo si el jugador se mueve o hace dash
                bool isMoving = GetMovementDirection().sqrMagnitude > 0.01f ||
                                playerDash.IsDashing();

                if (isMoving)
                {
                    ProcessHit(collision);
                }
            }
        }

        if (collision.CompareTag("ParryBall"))
        {
            if (!playerParry.IsParrying())
                ProcessHit(collision);
            else
            {
                ParryEffectManager.Instance.PlayParryEffect(collision.transform.position);
                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("EnemyBall") && !playerDash.IsDashing())
        {
            ProcessHit(collision);
        }
    }

    private void ProcessHit(Collider2D collision)
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
