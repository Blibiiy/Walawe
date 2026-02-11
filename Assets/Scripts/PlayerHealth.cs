using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    public float maxhealth = 100f;
    public float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;

    [Header("Game Over Reference")]
    public GameObject gameOverPanel;


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
        Debug.Log("Game Over");

        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // matikan skrip controller player
        GetComponent<PlayerMovement>().enabled = false; 


        // kembalikan cursor seperti semula
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // hentikan game
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
