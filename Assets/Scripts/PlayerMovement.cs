using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Transform playerTransform;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    [Header("Stamina System")]
    public float maxStamina = 100f;
    public float drainRate = 10f;
    public float regenRate = 8f;
    public float fatigueThreshold = 30f;

    [Header("Debug Info")]
    [SerializeField] float currentStamina;
    [SerializeField] bool canRun = true;
    [SerializeField] bool isGrounded;
    public bool isSprinting;


    // Variabel kalkulasi fisika
    private Vector3 velocity;      // Untuk gravitasi (Y axis)
    private Vector3 finalVelocity; // Untuk gerak horizontal
    private Vector3 momentum;      // "Ingatan" kecepatan saat terbang


    private float originalHeight;
    private Vector3 originalCenter;
    public float crouchHeight = 0.5f;
    public float crouchSpeed = 0f;
    public bool isCrouching = false;


    private void Start()
    {
        currentStamina = maxStamina;
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        HandleGravity();
        HandleMovementAndStamina();
        HandleCrouch();
    }

    // --- FUNGSI MODULAR ---

    void HandleGravity()
    {
        // Cek apakah kaki menyentuh layer 'Ground'
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset velocity gravitasi saat menapak (diberi -2f agar tetap menempel kuat di tanah/turunan)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Akumulasi gravitasi (Jatuh)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovementAndStamina()
    {
        // Safety check: Pastikan keyboard terhubung
        if (Keyboard.current == null) return;

        // 1. INPUT READING (Membaca tombol WASD)
        float x = 0;
        float z = 0;

        if (Keyboard.current.aKey.isPressed) x = -1;
        if (Keyboard.current.dKey.isPressed) x = 1;
        if (Keyboard.current.sKey.isPressed) z = -1;
        if (Keyboard.current.wKey.isPressed) z = 1;

        // 2. LOGIKA STATE & STAMINA (Dihitung di awal sebelum fisika)
        bool isMovingForward = z > 0;
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        // Tentukan status Sprint frame ini
        isSprinting = isShiftPressed && isMovingForward && canRun && currentStamina > 0;

        // Manajemen Pengurangan/Pemulihan Stamina
        if (isSprinting)
        {
            currentStamina -= drainRate * Time.deltaTime;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        // Logic Fatigue: Jika capek (<30), tidak bisa lari sampai pulih
        if (!isSprinting)
        {
            if (currentStamina < fatigueThreshold) canRun = false;
            else canRun = true;
        }

        // Kunci nilai stamina di antara 0 - 100
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // 3. KALKULASI FISIKA GERAK
        if (isGrounded)
        {
            // A. Logika Lompat (Impuls instan)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !isCrouching)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // B. Hitung Arah & Kecepatan
            Vector3 moveDirection = transform.right * x + transform.forward * z;

            // Normalize agar gerak diagonal tidak lebih cepat
            if (moveDirection.magnitude > 1f) moveDirection.Normalize();

            // Pilih speed berdasarkan status sprint yang sudah dihitung di atas
            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            if (isSprinting)
                targetSpeed = sprintSpeed;
            else if (isCrouching)
                targetSpeed = crouchSpeed;
            else 
                targetSpeed = walkSpeed;

            // Set output kecepatan
            finalVelocity = moveDirection * targetSpeed;
        }
        else
        {
            // C. Logika Udara: Gunakan momentum terakhir, abaikan input baru
            finalVelocity = momentum;
        }

        // 4. EKSEKUSI & PENYIMPANAN
        controller.Move(finalVelocity * Time.deltaTime);

        // Simpan momentum hanya saat di tanah. 
        // Y diset 0 agar momentum murni horizontal (mencegah loncat ganda tak sengaja)
        if (isGrounded)
        {
            momentum.Set(controller.velocity.x, 0, controller.velocity.z);
        }
    }





    void HandleCrouch()
    {
        if (Keyboard.current.leftCtrlKey.isPressed)
        {
            controller.height = crouchHeight;

            playerTransform.localScale = new Vector3(1, 0.5f, 1);
            controller.center = new Vector3(0, crouchHeight / 2f,0);
            canRun = false;
            isCrouching = true;
        }
        else
        {
            controller.height = originalHeight;
            playerTransform.localScale = new Vector3(1, 1, 1);
            controller.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z);
            canRun = true;
            isCrouching = false;
        }
    }
}