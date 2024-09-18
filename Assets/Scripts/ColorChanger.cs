using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorChanger : MonoBehaviour
{
    public GameObject plane; // The plane to change color
    public Material redMaterial, blueMaterial, greenMaterial, yellowMaterial; // Materials for plane colors

    private GameObject[] redBlocks, blueBlocks, greenBlocks, yellowBlocks; // Arrays for colored blocks

    void Start()
    {
        // Find blocks by their tags
        redBlocks = GameObject.FindGameObjectsWithTag("RedBlock");
        blueBlocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        greenBlocks = GameObject.FindGameObjectsWithTag("GreenBlock");
        yellowBlocks = GameObject.FindGameObjectsWithTag("YellowBlock");

        // Add debug logs to confirm block assignment
        Debug.Log("Red Blocks Found: " + redBlocks.Length);
        Debug.Log("Blue Blocks Found: " + blueBlocks.Length);
        Debug.Log("Green Blocks Found: " + greenBlocks.Length);
        Debug.Log("Yellow Blocks Found: " + yellowBlocks.Length);
    }

    public void ChangeColor(string color)
    {
        //Debug.Log("Color button clicked: " + color); // Debug log to track button clicks

        switch (color)
        {
            case "Red":
                plane.GetComponent<Renderer>().material = redMaterial;
                ToggleBlocks(redBlocks, false);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, true);
                break;

            case "Blue":
                plane.GetComponent<Renderer>().material = blueMaterial;
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, false);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, true);
                break;

            case "Green":
                plane.GetComponent<Renderer>().material = greenMaterial;
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, false);
                ToggleBlocks(yellowBlocks, true);
                break;

            case "Yellow":
                plane.GetComponent<Renderer>().material = yellowMaterial;
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, false);
                break;
        }
    }

    private void ToggleBlocks(GameObject[] blocks, bool isVisible)
    {
        foreach (GameObject block in blocks)
        {
            // Add debug logs to track which blocks are being toggled
            //Debug.Log("Toggling block: " + block.name + " Visibility: " + isVisible);

            // Instead of SetActive, disable the renderer and collider separately
            Renderer blockRenderer = block.GetComponent<Renderer>();
            Collider blockCollider = block.GetComponent<Collider>();

            if (blockRenderer != null) blockRenderer.enabled = isVisible;
            if (blockCollider != null) blockCollider.enabled = isVisible;
        }
    }
}
