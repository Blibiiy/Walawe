using TMPro;
using UnityEngine;

public class WallGun : MonoBehaviour, IInteractable
{
    public int ammoMount = 30;
    public int gunCost = 500;
    public AudioClip cantBuyGunSound;
    public AudioSource audiosource;
    
    public void Interact()
    {
        Weapon gun = FindFirstObjectByType<Weapon>();

        if (gun != null)
        {
            if(PointManager.instance.currentPoints >= gunCost)
            {
                PointManager.instance.ReducePoints(gunCost);
                gun.AddAmmo(ammoMount);
            }
            else
            {
                audiosource.PlayOneShot(cantBuyGunSound);
            }
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
