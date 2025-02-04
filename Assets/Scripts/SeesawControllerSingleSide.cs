using UnityEngine;

public class SeesawControllerSingleSide : MonoBehaviour
{
    public Ignitable leftIgnitable;    // Reference to the left Ignitable
    public float rotationAngle = 45f;  // Maximum rotation angle when ignited
    public float rotationSpeed = 2f;   // Speed of rotation

    private float targetAngle = 0f;    // Target rotation angle for the seesaw
    private Transform centerPivot;     // Empty GameObject that acts as the center pivot

    void Start()
    {
        // Create a center pivot at the center of the seesaw object
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            centerPivot = new GameObject("CenterPivot").transform;
            centerPivot.position = renderer.bounds.center; // Set to center of seesaw
            transform.SetParent(centerPivot); // Make the seesaw a child of the center pivot
        }
        else
        {
            //Debug.LogError("No Renderer found on this object or its children.");
        }
    }

    void Update()
    {
        // Check the ignition state of the left ignitable and set the target angle
        if (leftIgnitable != null && leftIgnitable.isYellowIgnited)
        {
            targetAngle = -rotationAngle;  // Rotate to positive angle when ignited
        }
        else
        {
            targetAngle = 0f;  // Return to horizontal when not ignited
        }

        // Smoothly rotate the centerPivot to the target angle using Quaternion.Lerp
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0f, 0f);
        centerPivot.localRotation = Quaternion.Lerp(centerPivot.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
