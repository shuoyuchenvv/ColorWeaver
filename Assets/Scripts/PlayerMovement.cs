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
        horizontalInput = 0;
        verticalInput = 0;
        velocity = Vector3.zero;
    }

    void Update()
    {
        GetPlayerInput();
        HandleMovement();
        HandleCameraControl();  
    }

    public void SetControlMode(bool useController)
    {
        isUsingController = useController;
        Debug.Log($"PlayerMovement control mode changed to: {(useController ? "Controller" : "Keyboard/Mouse")}");

        //Update new mode 
        GetPlayerInput();
    }

    private void HandleCameraControl()
    {
        Debug.Log("HandleCameraControl called");

        if (isUsingController)
        {
            Debug.Log("Using controller");

            // Right stick camera control
            float lookX = Input.GetAxis("Right_Horizontal") * mouseSensitivity * Time.deltaTime*100f;
            float lookY = Input.GetAxis("Right_Vertical") * mouseSensitivity * Time.deltaTime*100f;

            Debug.Log($"Right stick input: X={lookX}, Y={lookY}");

            if (Mathf.Abs(lookX) > 0.01f || Mathf.Abs(lookY) > 0.01f)  // 
            {
                currentX += lookX;
                currentY = Mathf.Clamp(currentY - lookY, -verticalAngleLimit, verticalAngleLimit);
                UpdateCameraPosition();
            }

            
        }
        else
        {
            Debug.Log("Using keyboard/mouse");

            // Mouse camera control
            if (Input.GetMouseButton(1))  // Right mouse button
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                currentX += mouseX;
                currentY = Mathf.Clamp(currentY - mouseY, -verticalAngleLimit, verticalAngleLimit);
            }
        }

        UpdateCameraPosition();
    }

    private void SetupInitialState()
    {

        // Get control type preference
        isUsingController = (PlayerPrefs.GetInt("ControlType", 0) == 1);
        Debug.Log($"Loading control type from PlayerPrefs: {PlayerPrefs.GetInt("ControlType", 0)}");
        Debug.Log($"isUsingController set to: {isUsingController}");
        /*
        // Get control type preference
        //isUsingController = PlayerPrefs.GetInt("ControlType", 0) == 1;
        if (SettingsManager.Instance != null && SettingsManager.Instance.controlTypeDropdown != null)
        {
            isUsingController = SettingsManager.Instance.controlTypeDropdown.value == 1;
            Debug.Log("ControlType from SettingsManager: " + SettingsManager.Instance.controlTypeDropdown.value);
        }
        else
        {
            Debug.LogError("SettingsManager or controlTypeDropdown reference not found!");
            isUsingController = false;  // 设置一个默认值
        }

        Debug.Log("IsUsingController: " + isUsingController);
        */


        // Setup cursor and camera
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();

        // Initialize camera position
        currentX = 180f;
        currentY = 10f;
        UpdateCameraPosition();

        //initialize input
        horizontalInput = 0;
        verticalInput = 0;
        velocity = Vector3.zero;
    }

    private void GetPlayerInput()
    {
        Debug.Log($"Control Mode: {(isUsingController ? "Controller" : "Keyboard/Mouse")}");

        if (isUsingController)
        {
            // Controller input
            //horizontalInput = Input.GetAxis("Horizontal"); // use basic for a while
            //verticalInput = Input.GetAxis("Vertical");     // use basic for a while
            horizontalInput = Input.GetAxis("Joy_Horizontal");
            verticalInput = Input.GetAxis("Joy_Vertical");
            jumpPressed = Input.GetButtonDown("Joy_Jump"); // mapped to ▲ button

            Debug.Log($"Controller Input - H: {horizontalInput}, V: {verticalInput}");

            Debug.Log($"Using Controller - Raw Input Values:");
            Debug.Log($"Horizontal: {Input.GetAxisRaw("Horizontal")}");
            Debug.Log($"Vertical: {Input.GetAxisRaw("Vertical")}");
            Debug.Log($"Joy_Horizontal: {Input.GetAxisRaw("Joy_Horizontal")}");
            Debug.Log($"Joy_Vertical: {Input.GetAxisRaw("Joy_Vertical")}");
        }
        else
        {
            // Keyboard/Mouse input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpPressed = Input.GetButtonDown("Jump");

            Debug.Log($"Keyboard Input - H: {horizontalInput}, V: {verticalInput}");
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


    // test
    void TestInputs()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float jh = Input.GetAxis("Joy_Horizontal");
        float jv = Input.GetAxis("Joy_Vertical");

        if (h != 0 || v != 0 || jh != 0 || jv != 0)
        {
            Debug.Log($"Input Test - K/M: ({h}, {v}), Controller: ({jh}, {jv})");
        }
    }
}
