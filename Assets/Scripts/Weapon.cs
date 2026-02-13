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
        float damage = normalDamage;

        if (Physics.Raycast(ray, out hitInfo, range))
        {
            IDamageable damageable = hitInfo.collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                HitZone zone = HitZone.Body;
                int layer = hitInfo.collider.gameObject.layer;
                if (layer == LayerMask.NameToLayer("Enemy Head"))
                {
                    damage = headDamage;
                    zone = HitZone.head;
                }
                DamageInfo info = new DamageInfo(damage, zone);

                damageable.TakeDamage(info);
                
            }
            else
            {
                GameObject newHole =  Instantiate(bulletHolePrefab, hitInfo.point + hitInfo.normal * 0.01f, Quaternion.LookRotation(- hitInfo.normal));

                Destroy(newHole, 5);
            }
        }
        Debug.Log("Ammo : " + currentAmmo);
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