using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float maxCameraDistance = 5f;  // Maximum distance from the player to the camera
    public float minCameraDistance = 1f;  // Minimum distance when near a wall
    public float smoothSpeed = 10f;       // Speed of smoothing (for faster response)
    public float mouseSensitivity = 100f; // Sensitivity for mouse movement
    public float verticalLimit = 80f;     // Limit for vertical rotation to prevent over-rotation
    public LayerMask collisionMask;       // LayerMask to specify which layers to collide with
    public float RaycastDistance = 5f;

    [Header("Transparency Settings")]
    public float transparentDistance = 5f;  // Distance threshold for player transparency
    private TransparencyController transparencyController; // Reference to transparency controller

    private float currentYaw = 0f;        // Yaw (horizontal rotation)
    private float currentPitch = 0f;      // Pitch (vertical rotation)
    private float currentCameraDistance;  // Current camera distance from the player
    private float targetCameraDistance;   // Target camera distance (smooth adjustment)
    private Vector3 desiredPosition;      // The desired camera position
    private bool isColliding = false;     // To track if we are colliding
    private bool isUsingController;

    void Start()
    {
        currentCameraDistance = maxCameraDistance; // Start with the maximum distance
        targetCameraDistance = maxCameraDistance;  // Set target distance to max initially
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor in the center of the screen
        isUsingController = PlayerPrefs.GetInt("ControlType", 0) == 1;

        // Get the TransparencyController component from the player
        transparencyController = player.GetComponent<TransparencyController>();

        if (player == null)
        {
            Debug.LogError("Player reference is missing in CameraFollow script.");
        }
        else if (transparencyController == null)
        {
            Debug.LogError("TransparencyController component is missing on the Player.");
        }

    }

    void LateUpdate()
    {
        HandleMouseInput(); // Mouse input controls rotation only
        HandleCameraCollision(); // Detect collisions and adjust camera distance
        UpdateCameraPosition();  // Update camera position smoothly

        // Check if the camera is within the transparency distance
        if (transparencyController != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < transparentDistance)
            {
                transparencyController.SetTemporaryTransparency(); // Set transparency when close
            }
            else
            {
                transparencyController.ResetTransparency(); // Reset to original opacity when far
            }
        }
    }

    // Handle mouse input to rotate the camera around the player
    void HandleMouseInput()
    {
        if (!isUsingController)  // only use mouse when mouse is on
        {
            currentYaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            currentPitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Clamp the vertical rotation to prevent over-rotation
            currentPitch = Mathf.Clamp(currentPitch, -verticalLimit, verticalLimit);
        }
    }

    // Detect collisions between the player and camera, adjust camera distance if necessary
    void HandleCameraCollision()
    {
        isColliding = false;
        Vector3 direction = (transform.position - player.position).normalized;
        RaycastHit hit;

        // Detect obstacles in front of the camera using SphereCast
        if (Physics.SphereCast(player.position, 0.5f, direction, out hit, maxCameraDistance + 0.5f, collisionMask))
        {
            float hitDistance = Vector3.Distance(player.position, hit.point);
            targetCameraDistance = Mathf.Clamp(hitDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }
        else
        {
            targetCameraDistance = maxCameraDistance;
        }

        // Perform additional collision checks
        CheckSurroundingCollisions();
    }

    // Additional checks for surrounding collisions (back, left, right, and downwards)
    void CheckSurroundingCollisions()
    {
        RaycastHit hit;

        // Improved backward collision detection (camera back towards player, with increased detection distance)
        if (Physics.Raycast(transform.position, -transform.forward, out hit, RaycastDistance, collisionMask))  // Increased detection range to 5.0f
        {
            //Debug.Log("Colliding with object behind camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);  // Immediate adjustment
            isColliding = true;
        }

        // Left collision detection
        if (Physics.Raycast(transform.position, -transform.right, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object to the left of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Right collision detection
        if (Physics.Raycast(transform.position, transform.right, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object to the right of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Downward collision detection (for ground or floor detection)
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with ground or object below camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Upward collision detection (for ceiling or above)
        if (Physics.Raycast(transform.position, Vector3.up, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object above camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Diagonal left-forward collision detection
        if (Physics.Raycast(transform.position, (-transform.right + transform.forward).normalized, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object diagonally left-forward of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Diagonal right-forward collision detection
        if (Physics.Raycast(transform.position, (transform.right + transform.forward).normalized, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object diagonally right-forward of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Diagonal left-backward collision detection
        if (Physics.Raycast(transform.position, (-transform.right - transform.forward).normalized, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object diagonally left-backward of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }

        // Diagonal right-backward collision detection
        if (Physics.Raycast(transform.position, (transform.right - transform.forward).normalized, out hit, RaycastDistance, collisionMask))
        {
            //Debug.Log("Colliding with object diagonally right-backward of camera: " + hit.collider.name);
            targetCameraDistance = Mathf.Clamp(targetCameraDistance - 0.1f, minCameraDistance, maxCameraDistance);
            isColliding = true;
        }


    }


    // Update the camera position based on the mouse input and collision detection
    void UpdateCameraPosition()
    {
        // Smoothly adjust the camera distance (interpolate between current and target distance)
        currentCameraDistance = Mathf.Lerp(currentCameraDistance, targetCameraDistance, Time.deltaTime * smoothSpeed);

        // Calculate the new camera rotation based on mouse input
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 direction = rotation * Vector3.back;

        // Calculate the new camera position based on the adjusted camera distance
        Vector3 targetPosition = player.position + direction * currentCameraDistance;

        // Smoothly move the camera to the new position
        transform.position = targetPosition;

        // Make the camera always look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust 1.5f to aim at the player's head height
    }
}
