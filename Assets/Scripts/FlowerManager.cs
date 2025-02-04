using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    public GameObject[] flowers; // Array of flower GameObjects
    private int currentFlowerIndex = 0; // Index to track the current flower in the sequence

    void Start()
    {
        // Only show the first flower at the start, hide the others
        for (int i = 0; i < flowers.Length; i++)
        {
            if (i == 0)
                flowers[i].SetActive(true); // Show the first flower
            else
                flowers[i].SetActive(false); // Hide the rest
        }
    }

    // Method to handle when the player touches a flower
    public void HandleFlowerTouched(GameObject flower)
    {
        // Check if the flower touched is the current one
        if (flowers[currentFlowerIndex] == flower)
        {
            flower.SetActive(false); // Hide the current flower

            // Move to the next flower if it exists
            currentFlowerIndex++;
            if (currentFlowerIndex < flowers.Length)
            {
                flowers[currentFlowerIndex].SetActive(true); // Show the next flower
            }
        }
    }
}
