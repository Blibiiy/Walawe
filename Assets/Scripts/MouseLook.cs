using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 50f; // Sensitivitas dasar
    public float sprintMultiplier = 0.5f; // Pengali saat lari (0.5 artinya jadi setengah lebih lambat)

    [Header("References")]
    public Transform playerBody;
    [SerializeField] PlayerMovement playerMovement;

    float xRotation = 0f;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Tentukan sensitivitas saat ini tanpa merusak nilai asli
        float currentSens = mouseSensitivity;

        // Cek apakah playerMovement ada isinya (mencegah error jika lupa assign)
        if (playerMovement != null && playerMovement.isSprinting)
        {
            currentSens *= sprintMultiplier; // Misal 50 * 0.5 = 25
        }

        // 2. Ambil Input
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Gunakan 'currentSens' hasil perhitungan tadi
        float mouseX = mouseDelta.x * currentSens * Time.deltaTime;
        float mouseY = mouseDelta.y * currentSens * Time.deltaTime;

        // 3. Eksekusi Rotasi
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}