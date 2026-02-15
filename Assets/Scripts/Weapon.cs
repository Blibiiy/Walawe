using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI ammoText;


    [Header("Gun Stats")]
    public float normalDamage = 10f;
    public float headDamage = 20f;
    public float range = 100f;
    public int maxAmmo = 10;
    public int reserveAmmo = 30;
    public float reloadTime = 1.5f;
    private int currentAmmo;
    private bool isReloading = false;

    [Header("Visual Effect")]
    public GameObject bulletHolePrefab;

    [Header("Audio")]
    public AudioSource audiosource;
    public AudioClip gunshotSound;
    public AudioClip gunreloadSound;
    public AudioClip killEnemy;


    public Camera fpsCam;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading)
            return;

        if (reserveAmmo > 0 && (Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < maxAmmo) || currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            audiosource.PlayOneShot(gunreloadSound);
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !isReloading)
        {
            Shoot();
        }


    }

    void OnEnable()
    {
        isReloading = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;

        int ammoToTake = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToTake;
        reserveAmmo -= ammoToTake;

        isReloading = false;
        
        UpdateAmmoUI();
    }

    void Shoot()
    {
        currentAmmo--;
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        audiosource.PlayOneShot(gunshotSound);

        if (Physics.Raycast(ray, out hitInfo, range))
        {

            IDamageable damageable = hitInfo.collider.gameObject.GetComponentInParent<IDamageable>();
            HitZoneIdentifier zone = hitInfo.collider.GetComponentInParent<HitZoneIdentifier>();
            Target target = hitInfo.collider.GetComponentInParent<Target>();

            if(damageable != null && zone != null)
            {
                DamageInfo info = new DamageInfo(normalDamage, zone.hitzone);
                damageable.TakeDamage(info);
                if(target != null)
                {
                    if (target.IsDeath())
                        audiosource.PlayOneShot(killEnemy);
                }
            }
            else
            {
                GameObject newHole =  Instantiate(bulletHolePrefab, hitInfo.point + hitInfo.normal * 0.01f, Quaternion.LookRotation(- hitInfo.normal));
                Destroy(newHole, 5);
            }
        }
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo + "/" + reserveAmmo;
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
        UpdateAmmoUI();
    }
}