using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CellGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color baseColor = Color.white;   
    public Color dangerColor = new Color(1f, 0f, 0f, 0.5f); 
    public float pulseSpeed = 2f;           
    public float pulseIntensity = 0.5f;     
    public float alphaPulse = 0.6f;       

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; 
        t = Mathf.Pow(t, 2.5f);  

        Color pulsingColor = Color.Lerp(baseColor, dangerColor, t * pulseIntensity);

        pulsingColor.a = Mathf.Lerp(0f, alphaPulse, t);

        image.color = pulsingColor;
    }
}
