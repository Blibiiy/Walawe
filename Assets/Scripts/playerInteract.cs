using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInteract : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public TextMeshProUGUI gunInfoText;

    void Update()
    {
        HandleRaycast();
    }

    void HandleRaycast()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactDistance, interactLayer))
        {

            gunInfoText.gameObject.SetActive(true);
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                IInteractable interractable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                if (interractable != null)
                {
                    interractable.Interact();
                }
            }
        }
        else
        {
            if (gunInfoText.gameObject.activeInHierarchy)
                gunInfoText.gameObject.SetActive(false);
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
        }
    }
}
