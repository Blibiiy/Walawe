using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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


    public GameObject gamePausePanel;


    
    private Vector3 velocity;      
    private Vector3 finalVelocity; 
    private Vector3 momentum;
    private Vector3 origin;


    private float originalHeight;
    private Vector3 originalCenter;
    public float crouchHeight = 0.5f;
    public float crouchSpeed = 0f;
    public bool isCrouching = false;
    public float rayLength;


    private void Start()
    {
        currentStamina = maxStamina;
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        HandleMovementAndStamina();
        HandleGravity();
        HandleCrouch();
        HandleGamePause();
        SlowTime();
    }

    // --- FUNGSI MODULAR ---

    void HandleGravity()
    {
        origin = controller.bounds.center;
        rayLength = controller.bounds.extents.y + groundDistance;

        float radius = controller.bounds.extents.x;

        //isGrounded = Physics.Raycast(origin, Vector3.down, rayLength, groundMask);

        isGrounded = Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, rayLength, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovementAndStamina()
    {
        if (Keyboard.current == null) return;

        float x = 0;
        float z = 0;

        if (Keyboard.current.aKey.isPressed) x = -1;
        if (Keyboard.current.dKey.isPressed) x = 1;
        if (Keyboard.current.sKey.isPressed) z = -1;
        if (Keyboard.current.wKey.isPressed) z = 1;

        bool isMovingForward = z > 0;
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        isSprinting = isShiftPressed && isMovingForward && canRun && currentStamina > 0;

        if (isSprinting)
        {
            currentStamina -= drainRate * Time.deltaTime;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        if (!isSprinting)
        {
            if (currentStamina < fatigueThreshold) canRun = false;
            else canRun = true;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (isGrounded)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !isCrouching)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            Vector3 moveDirection = transform.right * x + transform.forward * z;

            if (moveDirection.magnitude > 1f) moveDirection.Normalize();

            float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
            if (isSprinting)
                targetSpeed = sprintSpeed;
            else if (isCrouching)
                targetSpeed = crouchSpeed;
            else 
                targetSpeed = walkSpeed;

            finalVelocity = moveDirection * targetSpeed;
        }
        else
        {
            finalVelocity = momentum;
        }

        controller.Move(finalVelocity * Time.deltaTime);

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

            //playerTransform.localScale = new Vector3(1, 0.5f, 1);
            controller.center = new Vector3(0, crouchHeight / 2f,0);
            canRun = false;
            isCrouching = true;
        }
        else
        {
            controller.height = originalHeight;
            //playerTransform.localScale = new Vector3(1, 1, 1);
            controller.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z);
            canRun = true;
            isCrouching = false;
        }
    }


    void HandleGamePause()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && Time.timeScale == 1)
        {
            PauseGame();
        }

        else if (Keyboard.current.escapeKey.wasPressedThisFrame && Time.timeScale == 0)
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        gamePausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponentInChildren<Weapon>().enabled = false;
    }

    void SlowTime()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.timeScale == 1f)
            Time.timeScale = 0.5f;
        else if (Mouse.current.rightButton.wasPressedThisFrame && Time.timeScale == 0.5f)
            Time.timeScale = 1;
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        gamePausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponentInChildren<Weapon>().enabled = true;
    }

    public void ExitGame()
    {
        EditorApplication.isPlaying = false;
    }
}