using UnityEngine;

public class SeesawController : MonoBehaviour
{
    public Ignitable leftIgnitable;   // Reference to the left Ignitable
    public Ignitable rightIgnitable;  // Reference to the right Ignitable
    public float rotationAngle = 30f; // Maximum rotation angle for each side
    public float rotationSpeed = 2f;  // Speed of rotation

    private float targetAngle = 0f;   // Target rotation angle for the seesaw
    private Transform centerPivot;    // Empty GameObject that acts as the center pivot

    void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>(); // Get Renderer from child
        if (renderer != null)
        {
            centerPivot = new GameObject("CenterPivot").transform;
            centerPivot.position = renderer.bounds.center; // Use child's Renderer bounds
            transform.SetParent(centerPivot); // Make the seesaw a child of the center pivot
        }
        else
        {
            //Debug.LogError("No Renderer found on this object or its children.");
        }
    }

    void UpdateTargetAngle()
    {
        // Determine the target angle based on ignition state
        if (leftIgnitable != null && leftIgnitable.isYellowIgnited &&
            (rightIgnitable == null || !rightIgnitable.isYellowIgnited))
        {
            targetAngle = rotationAngle;  // Only left side ignited, rotate to positive angle
        }
        else if (rightIgnitable != null && rightIgnitable.isYellowIgnited &&
                 (leftIgnitable == null || !leftIgnitable.isYellowIgnited))
        {
            targetAngle = -rotationAngle; // Only right side ignited, rotate to negative angle
        }
        else
        {
            targetAngle = 0f;  // Both sides ignited or neither ignited, return to horizontal
        }
    }

    void Update()
    {
        // Update the target angle every frame to capture ignition changes
        UpdateTargetAngle();

        // Smoothly rotate the centerPivot to the target angle using Quaternion.Lerp
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0f, 0f);
        centerPivot.localRotation = Quaternion.Lerp(centerPivot.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Debugging output to monitor angle changes
        //Debug.Log("Target Angle: " + targetAngle);
        //Debug.Log("Current Rotation: " + centerPivot.localRotation.eulerAngles.x);
    }
}
