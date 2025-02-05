using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    public CharacterController controller;
    public CloudyLineDrawer linedrawer;
    public Transform playerCamera;
    public InstructionTutorial instructionTutorial;
    private Animator animator;

    [Header("Movement Settings")]
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float cameraDistance = 5f;
    public float verticalAngleLimit = 80f;

    [Header("Fart Settings")]
    public float fartDelayTime = 0.2f;
    public float fartOffSet = 0.1f;

    [Header("State Variables")]
    public bool isWindTop = false;
    public bool isLifting = false;
    public float yellowMaxHeight = 144f;

    private Vector3 velocity;
    private bool isGrounded;
    private float currentX = 0f;
    private float currentY = 0f;
    private bool isGravityEnabled = true;

    // Input variables
    private float horizontalInput;
    private float verticalInput;
    private bool jumpPressed;
    private bool isUsingController;

    void Start()
    {
        SetupInitialState();
    }

    void Update()
    {
        GetPlayerInput();
        HandleMovement();
    }

    private void SetupInitialState()
    {
        // Get control type preference
        isUsingController = PlayerPrefs.GetInt("ControlType", 0) == 1;

        // Setup cursor and camera
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();

        // Initialize camera position
        currentX = 180f;
        currentY = 10f;
        UpdateCameraPosition();
    }

    private void GetPlayerInput()
    {
        if (isUsingController)
        {
            // Controller input
            horizontalInput = Input.GetAxis("Joy_Horizontal");
            verticalInput = Input.GetAxis("Joy_Vertical");
            jumpPressed = Input.GetButtonDown("Joy_Jump"); // Usually mapped to B button
        }
        else
        {
            // Keyboard/Mouse input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpPressed = Input.GetButtonDown("Jump");
        }
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 cameraPos = transform.position - (rotation * Vector3.forward * cameraDistance);
        playerCamera.position = cameraPos;
        playerCamera.LookAt(transform.position + Vector3.up * 1.5f);
    }

    void HandleMovement()
    {
        // Update grounded state
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Calculate movement vector based on camera direction
        Vector3 move = playerCamera.forward * verticalInput + playerCamera.right * horizontalInput;
        move.y = 0f;
        controller.Move(move * speed * Time.deltaTime);

        // Update animation
        animator.SetFloat("Speed", move.magnitude);

        // Rotate player towards movement direction
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Handle jumping
        if ((isGrounded || isWindTop) && jumpPressed && instructionTutorial.jumpActivated)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false;

            if (linedrawer.energyBar.HasEnoughEnergy(linedrawer.energyCostPerSet) && !linedrawer.energyBar.isRegenerating)
            {
                StartCoroutine(DelayedFart(horizontalInput, verticalInput));
            }
        }

        // Apply gravity
        if (isGravityEnabled)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator DelayedFart(float x, float z)
    {
        yield return new WaitForSeconds(fartDelayTime);
        if (Mathf.Abs(x) <= fartOffSet && Mathf.Abs(z) <= fartOffSet)
        {
            linedrawer.JustFart();
        }
    }

    public void ApplyLift(float liftForce, float windTopHeight)
    {
        if (!isLifting) return;

        float distanceToMaxHeight = windTopHeight - transform.position.y;
        isGravityEnabled = false;

        if (distanceToMaxHeight > 5f)
        {
            velocity.y = liftForce;
        }
        else if (distanceToMaxHeight <= 5f && distanceToMaxHeight > -5)
        {
            float lerpFactor = Mathf.InverseLerp(5f, 0f, distanceToMaxHeight);
            velocity.y = Mathf.Lerp(liftForce, 0, lerpFactor);
            isWindTop = true;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void StopLift()
    {
        isLifting = false;
        isGravityEnabled = true;
        isWindTop = false;

        if (!controller.isGrounded)
        {
            Debug.Log("Player falling after lift stopped.");
        }
        else
        {
            velocity.y = 0;
        }
    }
}
