using UnityEngine;

public class AmmoBox : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Ammo 30++");
        Destroy(gameObject);
    }
    
    public string GetDescription()
    {
        return "Pick Up Ammo";
    }
}
