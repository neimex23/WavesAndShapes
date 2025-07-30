using UnityEngine;
using System.Collections;

public class ChargingEnemyBall : MonoBehaviour
{
    [Header("References")]
    public Transform fillSprite;    // Hijo que se expande
    public Transform baseSprite;    // Hijo que define la escala máxima

    [Header("Charging Settings")]
    public float chargeTime = 2f;   // Tiempo para cargar
    public Vector3 maxScale = Vector3.zero; // Si es (0,0,0), usará la escala de baseSprite

    [Header("Pop Effect")]
    public float popScale = 1.2f;   // Escala máxima del pop
    public float popDuration = 0.15f; // Duración del efecto pop

    private float timer = 0f;
    private bool charged = false;
    private Collider2D hitbox;
    private Vector3 finalScale;

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        if (hitbox != null)
            hitbox.enabled = false;

        if (fillSprite != null)
            fillSprite.localScale = Vector3.zero;

        if (maxScale == Vector3.zero && baseSprite != null)
            finalScale = baseSprite.localScale;
        else
            finalScale = maxScale;
    }

    private void Update()
    {
        if (!charged)
            ChargeProcess();
    }

    private void ChargeProcess()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / chargeTime);

        if (fillSprite != null)
            fillSprite.localScale = Vector3.Lerp(Vector3.zero, finalScale, t);

        if (t >= 1f)
            ActivateBall();
    }

    private void ActivateBall()
    {
        charged = true;
        if (hitbox != null)
            hitbox.enabled = true;

        if (fillSprite != null)
            StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        Vector3 startScale = finalScale;
        Vector3 popTarget = finalScale * popScale;

        float t = 0f;
        // Expande hacia fuera
        while (t < 1f)
        {
            t += Time.deltaTime / (popDuration / 2f);
            fillSprite.localScale = Vector3.Lerp(startScale, popTarget, t);
            yield return null;
        }

        t = 0f;
        // Vuelve al tamaño final
        while (t < 1f)
        {
            t += Time.deltaTime / (popDuration / 2f);
            fillSprite.localScale = Vector3.Lerp(popTarget, finalScale, t);
            yield return null;
        }
    }
}
