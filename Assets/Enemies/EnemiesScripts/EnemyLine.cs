using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyLine : MonoBehaviour
{
    public enum LineType { Damage, Block }

    [Header("Line Appearance")]
    public LineType lineType = LineType.Damage;
    [Tooltip("Width of the line in world units")]
    public float lineWidth = 0.2f;

    [Header("Damage Line Colors")]
    public Color damageWarningColor = new Color(1f, 0f, 0f, 0.3f);
    public Color damageActiveColor = Color.red;

    [Header("Block Line Colors")]
    public Color blockWarningColor = new Color(0f, 0.5f, 1f, 0.3f);
    public Color blockActiveColor = Color.blue;

    [Header("Timing")]
    [Tooltip("Seconds before the line becomes active")]
    public float warningDuration = 1.5f;

    private LineRenderer lr;
    private BoxCollider2D col;
    private float timer;
    private bool active;

    private Vector2 p0;
    private Vector2 p1;

    private void Awake()
    {
        // Asegurar que el LineRenderer existe
        lr = GetComponent<LineRenderer>();
        if (lr == null)
            lr = gameObject.AddComponent<LineRenderer>();

        // Asegurar que el BoxCollider2D existe
        col = GetComponent<BoxCollider2D>();
        if (col == null)
            col = gameObject.AddComponent<BoxCollider2D>();

        // Configurar material del LineRenderer
        if (lr.material == null)
            lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        // Aseguramos que la línea esté debajo del Player
        // Suponiendo que el Player tiene sortingOrder = 0 o superior
        lr.sortingOrder = -1;

        // Configurar collider
        col.isTrigger = true;
        col.size = Vector2.one * 0.01f; // Placeholder inicial
        col.enabled = false;

        ApplyInitialColor();

        // Asignar tag automáticamente
        gameObject.tag = "EnemyLine";
    }

    private void Update()
    {
        if (active) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / warningDuration);

        Color a = lineType == LineType.Damage ? damageWarningColor : blockWarningColor;
        Color b = lineType == LineType.Damage ? damageActiveColor : blockActiveColor;
        Color current = Color.Lerp(a, b, t);

        lr.material.color = current;
        lr.startColor = current;
        lr.endColor = current;

        if (timer >= warningDuration)
            Activate();
    }

    /// <summary>Sets the two endpoints of the line in world space.</summary>
    public void SetPositions(Vector2 start, Vector2 end)
    {
        p0 = start;
        p1 = end;

        lr.SetPosition(0, p0);
        lr.SetPosition(1, p1);

        ResizeCollider();
    }

    private void ResizeCollider()
    {
        Vector2 center = (p0 + p1) * 0.5f;
        transform.position = center;

        Vector2 dir = (p1 - p0).normalized;
        float length = Vector2.Distance(p0, p1);

        col.size = new Vector2(length, lineWidth);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ApplyInitialColor()
    {
        Color c = lineType == LineType.Damage ? damageWarningColor : blockWarningColor;
        lr.material.color = c;
        lr.startColor = c;
        lr.endColor = c;
    }

    private void Activate()
    {
        active = true;
        col.enabled = true;
    }
}