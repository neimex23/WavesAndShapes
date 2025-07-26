using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHitFeedback : MonoBehaviour
{
    [Header("Sound Control")]
    public AudioClip hitSound;

    [Header("Hit Feedback Settings")]
    public float hitScale = 1.3f;
    public float scaleDuration = 0.1f;
    public float invincibilityDuration = 1f;
    public float blinkInterval = 0.1f;

    [Header("Knockback Settings")]
    public float knockbackForce = 12f;       // Fuerza del retroceso
    public float knockbackTime = 0.15f;     // Duración del retroceso
    public float rotationSpeed = 720f;      // Velocidad de rotación en grados/segundo

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private bool isInvincible = false;
    private Vector3 knockbackDirection;
    private Vector3 startHitPosition;
    private Quaternion startRotation;

    private Coroutine hitCoroutine; // Controla si ya hay un hit en curso

    private AudioSource audioSource;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void TriggerHit(Vector3 sourcePosition)
    {
        if (isInvincible) return;  
        isInvincible = true;

        if (hitSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(hitSound);
        }

        knockbackDirection = (transform.position - sourcePosition).normalized;
        hitCoroutine = StartCoroutine(HitAnimation());
    }


    private IEnumerator HitAnimation()
    {
        isInvincible = true;
        yield return StartCoroutine(DoKnockback());

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
        hitCoroutine = null;
    }

    private IEnumerator DoKnockback()
    {
        StartCoroutine(ScaleBounce());

        float elapsed = 0f;
        while (elapsed < knockbackTime)
        {
            transform.position += knockbackDirection * knockbackForce * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ScaleBounce()
    {
        Vector3 targetScale = originalScale * hitScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / scaleDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / scaleDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
    }

    public bool IsInvincible() => isInvincible;
}
