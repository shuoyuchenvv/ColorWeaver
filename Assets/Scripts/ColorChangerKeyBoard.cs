using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for push I added this line
public class ColorChangerKeyBoard : MonoBehaviour
{
    public Camera mainCamera;
    public Material redMaterial, blueMaterial, greenMaterial, yellowMaterial;
    public InstructionTutorial instructionTutorial;
    public GameObject player; // Reference to the player for color change
    public Color redColor, blueColor, greenColor, yellowColor;
    public AudioClip popSound; // Pop sound when changing color
    //private GameObject[] redBlocks, blueBlocks, greenBlocks, yellowBlocks;
    public List<GameObject> redBlocks = new List<GameObject>();
    public List<GameObject> blueBlocks = new List<GameObject>();
    public List<GameObject> greenBlocks = new List<GameObject>();
    public List<GameObject> yellowBlocks = new List<GameObject>();

    //add npc tags
    public List<GameObject> redNPC = new List<GameObject>();
    public List<GameObject> blueNPC = new List<GameObject>();
    public List<GameObject> greenNPC = new List<GameObject>();
    public List<GameObject> yellowNPC = new List<GameObject>();
    
    
    private AudioSource audioSource;
    private string currentColor;

    void Start()
    {
        //redBlocks = GameObject.FindGameObjectsWithTag("RedBlock");
        //blueBlocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        //greenBlocks = GameObject.FindGameObjectsWithTag("GreenBlock");
        //yellowBlocks = GameObject.FindGameObjectsWithTag("YellowBlock");

        redBlocks.AddRange(GameObject.FindGameObjectsWithTag("RedBlock"));
        blueBlocks.AddRange(GameObject.FindGameObjectsWithTag("BlueBlock"));
        greenBlocks.AddRange(GameObject.FindGameObjectsWithTag("GreenBlock"));
        yellowBlocks.AddRange(GameObject.FindGameObjectsWithTag("YellowBlock"));

        //NPC
        redNPC.AddRange(GameObject.FindGameObjectsWithTag("RedNPC"));
        blueNPC.AddRange(GameObject.FindGameObjectsWithTag("BlueNPC"));
        greenNPC.AddRange(GameObject.FindGameObjectsWithTag("GreenNPC"));
        yellowNPC.AddRange(GameObject.FindGameObjectsWithTag("YellowNPC"));

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check for number key input and change the background and player color accordingly
        if (Input.GetKeyDown(KeyCode.Alpha1) && instructionTutorial.redPeriodActivated) { ChangeColor("Red"); }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && instructionTutorial.bluePeriodActivated) { ChangeColor("Blue"); }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && instructionTutorial.greenPeriodActivated) { ChangeColor("Green"); }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && instructionTutorial.yellowPeriodActivated) { ChangeColor("Yellow"); }
    }

    // Method to change both background color and player's color based on input
    public void ChangeColor(string color)
    {
        currentColor = color;
        switch (color)
        {
            case "Red":
                mainCamera.backgroundColor = redColor;
                SetPlayerColor(redColor);
                Debug.Log("Changed to Red");
                ToggleBlocks(redBlocks, false);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, true);
                ToggleBlocks(redNPC, true);
                ToggleBlocks(blueNPC, false);
                ToggleBlocks(greenNPC, false);
                ToggleBlocks(yellowNPC, false);
                break;
            case "Blue":
                mainCamera.backgroundColor = blueColor;
                SetPlayerColor(blueColor);
                Debug.Log("Changed to Blue");
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, false);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, true);
                ToggleBlocks(redNPC, false);
                ToggleBlocks(blueNPC, true);
                ToggleBlocks(greenNPC, false);
                ToggleBlocks(yellowNPC, false);
                break;
            case "Green":
                mainCamera.backgroundColor = greenColor;
                SetPlayerColor(greenColor);
                Debug.Log("Changed to Green");
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, false);
                ToggleBlocks(yellowBlocks, true);
                ToggleBlocks(redNPC, false);
                ToggleBlocks(blueNPC, false);
                ToggleBlocks(greenNPC, true);
                ToggleBlocks(yellowNPC, false);
                break;
            case "Yellow":
                mainCamera.backgroundColor = yellowColor;
                SetPlayerColor(yellowColor);
                Debug.Log("Changed to Yellow");
                ToggleBlocks(redBlocks, true);
                ToggleBlocks(blueBlocks, true);
                ToggleBlocks(greenBlocks, true);
                ToggleBlocks(yellowBlocks, false);
                ToggleBlocks(redNPC, false);
                ToggleBlocks(blueNPC, false);
                ToggleBlocks(greenNPC, false);
                ToggleBlocks(yellowNPC, true);
                break;
        }
        audioSource.PlayOneShot(popSound);
    }

    // Method to return the current color (can be used for other purposes)
    public string GetCurrentColor()
    {
        return currentColor;
    }

    // Method to set the player's color, preserving the current transparency
    private void SetPlayerColor(Color color)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            Material material = playerRenderer.material; // Fetch the player's material
            Color playerColor = material.color;

            // Set RGB values based on the selected color, preserving the current alpha value for transparency
            playerColor.r = color.r;
            playerColor.g = color.g;
            playerColor.b = color.b;
            playerColor.a = material.color.a; // Keep current transparency

            material.color = playerColor;
        }
    }
    private void ToggleBlocks(List<GameObject> blocks, bool isVisible)
    {
        foreach (GameObject block in blocks)
        {
            // Add debug logs to track which blocks are being toggled
            //Renderer blockRenderer = block.GetComponent<Renderer>();
            //Collider blockCollider = block.GetComponent<Collider>();

            //if (blockRenderer != null) blockRenderer.enabled = isVisible;
            //if (blockCollider != null) blockCollider.enabled = isVisible;
            block.SetActive(isVisible);
            //Debug.Log($"{block.name} is active: {block.activeSelf}");

        }
    }
}
