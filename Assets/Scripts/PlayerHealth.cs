using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    public float maxhealth = 100f;
    public float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;

    private void Start()
    {
        currentHealth = maxhealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER");

        Time.timeScale = 0;
    }
}
