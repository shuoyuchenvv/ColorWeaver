using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoAppear : MonoBehaviour
{
    // The tomato object to be activated
    public GameObject tomato;

    // Reference to the script that handles color changes
    public ColorChangerKeyBoard colorChangerKeyBoard;
    public Ignitable ignitable;
    public GameObject OnScreenUI;
    // Holds the current color effect
    private string currentEffectColor;

    // Tag for the red color
    public string RedTag;

    // Called when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Debug log to confirm the collision
        Debug.Log("Collision detected with: " + other.name);

        // Get the current effect color
        currentEffectColor = colorChangerKeyBoard.GetCurrentColor();
        Debug.Log("Current color: " + currentEffectColor);

        // Check if the colliding object is the Player and if the color is red
        if (other.CompareTag("Player") && currentEffectColor == "Red")
        {
            // Activate the tomato object and set its tag
            tomato.SetActive(true);
            Ignitable.redActived = true;
            OnScreenUI.SetActive(true);
            tomato.tag = RedTag;
            colorChangerKeyBoard.redNPC.Add(tomato);
            
            // Debug log to confirm the tomato activation
            Debug.Log("Tomato activated and tag set to: " + RedTag);
        }
    }
}
