using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    public float maxhealth = 100f;
    public float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;

    [Header("Game Over Reference")]
    public GameObject gameOverPanel;

    public Volume globalVolume;
    private Vignette _vignette;


    private void Start()
    {
        // 1. metode try get, menggunakan out
        if (globalVolume.profile.TryGet<Vignette>(out _vignette))
        {
            _vignette.active = true;
        }

        // 2. metode biasanya yang nek tahu
        //_vignette = globalVolume.profile.GetComponent<Vignette>();
        //if(_vignette != null )
        //{
        //    _vignette.active = true;
        //}

        // kesimpulan, kode berjalan seperti biasa, tapi seperti yang dilihat, penggunaan tryget membuat kode yang lebih efisien dan rapi.





        currentHealth = maxhealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(DamageInfo info)
    {
        currentHealth -= info.BaseDamage;

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

    void UpdateDamgeEffect()
    {
        // TODO : add UI logic based on player's health
    }
}
