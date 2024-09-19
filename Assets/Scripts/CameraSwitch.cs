using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera MainCamera; // Reference to the left camera
    public Camera SideCamera_1; // Reference to the right camera

    private bool isLeftCameraActive = true; // Start with left camera active

    void Start()
    {
        // Set the initial state of the cameras
        MainCamera.enabled = true;
        SideCamera_1.enabled = false;
    }

    void Update()
    {
        // Detect Shift key press
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SwitchCamera();
        }
    }

    // Method to switch cameras
    void SwitchCamera()
    {
        if (isLeftCameraActive)
        {
            MainCamera.enabled = false;
            SideCamera_1.enabled = true;
        }
        else
        {
            MainCamera.enabled = true;
            SideCamera_1.enabled = false;
        }

        // Toggle the active camera flag
        isLeftCameraActive = !isLeftCameraActive;
    }
}
