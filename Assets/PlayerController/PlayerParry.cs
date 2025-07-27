using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [Header("Sound Control")]
    public AudioClip parrySound;

    [Header("Parry Settings")]
    public float parryWindow = 0.3f;   
    public float parryCooldown = 1f;   
    public GameObject shieldVisual;
    public event System.Action OnParry;

    private bool isParrying = false;
    private float cooldownTimer = 0f;
    private AudioSource audioSource;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Parry.performed += _ => TryParry();
        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    private void TryParry()
    {
        if (cooldownTimer <= 0f)
        {
            StartCoroutine(DoParry());
            cooldownTimer = parryCooldown;
            OnParry?.Invoke();

            if (audioSource != null && parrySound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(parrySound);
            }
        }
    }

    private System.Collections.IEnumerator DoParry()
    {
        isParrying = true;
        if (shieldVisual != null)
            shieldVisual.SetActive(true);


        yield return new WaitForSeconds(parryWindow);

        isParrying = false;
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    public bool IsParrying() => isParrying;
}
