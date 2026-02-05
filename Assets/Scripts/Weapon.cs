using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI ammoText;


    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;
    public int maxAmmo = 10;
    public float reloadTime = 1.5f;
    private int currentAmmo;
    private bool isReloading = false;


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

        if ((Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < maxAmmo) || currentAmmo <= 0)
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

        currentAmmo = maxAmmo;
        isReloading = false;
        
        UpdateAmmoUI();
    }

    void Shoot()
    {
        currentAmmo--;
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
            IDamageable damageable = hitInfo.collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        Debug.Log("Ammo : " + currentAmmo);
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo + "/" + maxAmmo;
    }
}