using UnityEngine;

public class ParryEffectManager : MonoBehaviour
{
    public static ParryEffectManager Instance;
    public GameObject parryEffectPrefab; // Prefab con el ParticleSystem

    private void Awake()
    {
        Instance = this;
    }

    public void PlayParryEffect(Vector3 position)
    {
        if (parryEffectPrefab != null)
        {
            GameObject effect = Instantiate(parryEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 1f); 
        }
    }
}
