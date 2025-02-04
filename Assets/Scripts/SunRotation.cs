using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float rotationSpeed = 10f; // Rotation speed of the light around the tower

    void Update()
    {
        // Rotate the SunPivot around the Y-axis (upward) at a constant speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
