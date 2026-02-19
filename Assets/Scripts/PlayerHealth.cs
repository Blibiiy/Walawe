using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.Audio;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    public float maxhealth = 100f;
    public float currentHealth;


    public Slider debugSlider;
    public float debugElapsed;
    public float debugDuration;
    public float startValue;
    public float targetValue;
    public bool canDebug;


    [Header("Game Over Reference")]
    public GameObject gameOverPanel;
    

    [Header("health Interface")]
    public Volume globalVolume;
    private Vignette _vignette;
    public Slider healthSlider;
    [SerializeField] private AudioMixer masterMixer;


    [Header("Healing Stats")]
    [SerializeField] private float hpRegen = 10f;
    [SerializeField] private float cdRegen = 10f;
    [SerializeField] private float waitForRegen = 0;
    [SerializeField] private bool canRegen = false;
    [SerializeField] private float elapsed = 0f;
    [SerializeField] private float duration = 0.2f;
     
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


    private void Update()
    {
        waitForRegen -= Time.deltaTime;
        canRegen = waitForRegen <= 0 ? true : false;
        if(canRegen)
        {
            if(currentHealth < maxhealth)
            {
                currentHealth = Mathf.Clamp(currentHealth + hpRegen * Time.deltaTime, 0, maxhealth);
                Healing();
            }
            
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * hpRegen);

            if (elapsed < duration)
            {
                elapsed += Time.deltaTime;


            }
        }

        if (currentHealth >= maxhealth)
            canRegen = false;


        //if (debugElapsed < debugDuration)
        //{
        //    debugElapsed += Time.deltaTime;
        //    float t = debugElapsed / debugDuration;
        //    debugSlider.value = Mathf.Lerp(startValue, targetValue + hpRegen, t);
        //    UpdateDamageEffect();
        //}

        //if (canDebug)
        //{
        //    debugElapsed = 0;
        //    canDebug = false;
        //}

    }

    public void TakeDamage(DamageInfo info)
    {
        currentHealth = Mathf.Clamp(currentHealth - info.BaseDamage, 0, maxhealth);

        waitForRegen = cdRegen;
        elapsed = 0;

        DamagedAndHealingEffect();

        SetMuffledEffect();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void DamagedAndHealingEffect()
    {
        if (healthSlider != null && masterMixer != null)
        {
            healthSlider.value = currentHealth;
            UpdateDamageEffect();
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

    void UpdateDamageEffect()
    {
        float healthPercent = currentHealth / maxhealth;

        float intensity = 1f - healthPercent;

        Debug.Log(healthPercent + " " + intensity);

        _vignette.intensity.value = Mathf.Clamp(intensity * 0.5f, 0, 0.5f);

    }

    public void SetMuffledEffect()
    {
        float healthPercent = currentHealth / maxhealth;
        float frequency = Mathf.Lerp(10, 22000f, healthPercent);

        masterMixer.SetFloat("MyLowPass", frequency);
    }



    void Healing()
    {

        UpdateDamageEffect();
        SetMuffledEffect();
        UpdateDamageEffect();
    }
}

