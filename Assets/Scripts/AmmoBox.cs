using UnityEngine;

public class AmmoBox : MonoBehaviour, IInteractable
{
    public int ammoMount = 30;
    public void Interact()
    {
        Weapon gun = FindFirstObjectByType<Weapon>();

        if (gun != null)
        {
            gun.AddAmmo(ammoMount);

            Destroy(gameObject);
        }

        else
        {
            Debug.LogError("Senjata tidak ditemukan!");
        }
    }
    
    public string GetDescription()
    {
        return "Pick Up Ammo";
    }
}
