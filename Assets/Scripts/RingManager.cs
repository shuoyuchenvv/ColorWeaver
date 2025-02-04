using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public GameObject bird;    // The bird object that appears after completing all rings
    public GameObject OnScreenUI;
    private int currentColorIndex = 0; // Index to track the current color cycle
    private string[] colors = { "Red", "Blue", "Green", "Yellow" }; // Color cycle
    private int ringsPassed = 0; // Track the number of rings passed
    private ColorChangerKeyBoard colorChangerKeyBoard;
    public Ignitable ignitable;

    private Dictionary<string, List<GameObject>> ringGroups = new Dictionary<string, List<GameObject>>();

    public ColorChangerKeyBoard colorChanger; // Reference to the ColorChangerKeyBoard to check the current color

    void Start()
    {
        // Initialize the dictionary with lists of rings by color
        ringGroups["Red"] = new List<GameObject>(GameObject.FindGameObjectsWithTag("RedRing"));
        ringGroups["Blue"] = new List<GameObject>(GameObject.FindGameObjectsWithTag("BlueRing"));
        ringGroups["Green"] = new List<GameObject>(GameObject.FindGameObjectsWithTag("GreenRing"));
        ringGroups["Yellow"] = new List<GameObject>(GameObject.FindGameObjectsWithTag("YellowRing"));    
        
        SetRingsActiveByColor("Red");
        bird.SetActive(false); // Hide the bird at the start
        

        colorChangerKeyBoard = FindObjectOfType<ColorChangerKeyBoard>();
        
    }

    // Method to set rings active/inactive by color
    void SetRingsActiveByColor(string color)
    {
        foreach (var ringGroup in ringGroups)
        {
            bool activate = ringGroup.Key == color;
            foreach (var ring in ringGroup.Value)
            {
                ring.SetActive(activate);
            }
        }
    }

    // Method to trigger when the player passes through the correct ring
    public void HandleRingPassed(GameObject ring)
    {
        string ringTag = ring.tag.Replace("Ring", "");

        // Check if the player's current background color matches the ring's color
        if (ringTag == colors[currentColorIndex] && ringTag == colorChanger.GetCurrentColor())
        {
            Debug.Log("Player color matches ring color!");
            ring.SetActive(false); // Deactivate the ring
            ringsPassed++;

            // Check if all rings of the current color are passed
            if (ringsPassed >= ringGroups[colors[currentColorIndex]].Count)
            {
                ringsPassed = 0; // Reset for the next color
                currentColorIndex++;

                // Check if all colors are passed (end of the puzzle)
                if (currentColorIndex >= colors.Length)
                {
                    ShowBird(); // Show the bird when the puzzle is complete
                }
                else
                {
                    SetRingsActiveByColor(colors[currentColorIndex]);
                }
            }
        }
        else
        {
            Debug.Log("Player color does not match ring color, cannot pass through.");
        }
    }

    // Show the bird object after completing the puzzle
    void ShowBird()
    {
        bird.SetActive(true);
        OnScreenUI.SetActive(true);
        Debug.Log("Bird has appeared!");
        colorChangerKeyBoard.yellowNPC.Add(bird);
        Ignitable.yellowActived=true;
    }
}
