using UnityEngine;

public class PlayerHealth: MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 5;
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ResetHeal()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        gameObject.SetActive(false);
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}
