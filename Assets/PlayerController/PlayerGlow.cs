using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerGlow : MonoBehaviour
{
    [Header("Idle Glow Settings")]
    public Color idleGlowColor = Color.white; 
    public float glowSpeed = 0.5f;            
    public float minIntensity = 0.7f;       
    public float maxIntensity = 1.2f;  

    private SpriteRenderer sprite;
    private Color originalColor;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    private void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * glowSpeed, 1f));
        sprite.color = idleGlowColor * intensity;
    }
}
