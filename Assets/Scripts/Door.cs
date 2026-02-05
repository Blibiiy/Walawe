using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "pintu";
    }

    public void Interact()
    {
        Debug.Log("Pintu terbuka");
    }
}
