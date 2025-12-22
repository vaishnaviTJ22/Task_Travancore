using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
