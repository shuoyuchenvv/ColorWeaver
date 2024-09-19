using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyLineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float energyCostPerSet = 10f; // Cost per set of spheres
    public int spheresPerSet = 3; // Number of spheres in one set of the track
    public float horizontalSpacing = 0.5f; // Spacing between horizontal spheres
    public GameObject sphereColliderPrefab; // Prefab of a sphere collider
    public Vector3 offset = new Vector3(0, -1, 0); // Offset to draw the line from the bottom of the player
    public float sphereVerticalOffset = 0.5f; // Vertical offset to align sphere tops with the bottom of the track

    private EnergyBar energyBar;
    private List<Vector3> positions = new List<Vector3>(); // Store positions during the drawing
    private bool isRecording = false;

    void Start()
    {
        energyBar = GetComponent<EnergyBar>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // Start recording positions when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return) && energyBar.HasEnoughEnergy(energyCostPerSet)) // Check if there's enough energy to start
        {
            StartRecording();
        }

        // Record positions while holding Enter
        if (Input.GetKey(KeyCode.Return) && isRecording)
        {
            Record();
        }

        // When Enter is released, create the cloudy line
        if (Input.GetKeyUp(KeyCode.Return))
        {
            CreateCloudyLine();
        }
    }

    void StartRecording()
    {
        isRecording = true;
        positions.Clear();
        Vector3 startPosition = transform.position + offset; // Adjust start position with offset
        positions.Add(startPosition);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPosition);
    }

    void Record()
    {
        // Record positions
        Vector3 currentPosition = transform.position + offset; // Adjust position with offset
        if (Vector3.Distance(positions[positions.Count - 1], currentPosition) > 0.5f) // Adjust spacing as needed
        {
            positions.Add(currentPosition);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
        }
    }

    void CreateCloudyLine()
    {
        isRecording = false;

        // Calculate total sets and the total cost
        int totalSets = positions.Count / spheresPerSet;
        float totalCost = totalSets * energyCostPerSet;

        // Determine how many sets can be created with the remaining energy
        int setsToGenerate = Mathf.Min(totalSets, Mathf.FloorToInt(energyBar.CurrentEnergy / energyCostPerSet));

        // Deduct energy for the sets that can be created
        if (setsToGenerate > 0)
        {
            float energyToUse = setsToGenerate * energyCostPerSet;
            energyBar.UseEnergy(energyToUse);

            // Instantiate colliders along the recorded positions
            for (int i = 0; i < setsToGenerate * spheresPerSet; i += spheresPerSet)
            {
                // Create a set of colliders for each recorded position
                for (int j = 0; j < spheresPerSet; j++)
                {
                    if (i + j < positions.Count)
                    {
                        Vector3 basePosition = positions[i + j];

                        // Create multiple spheres horizontally
                        for (int k = 0; k < spheresPerSet; k++)
                        {
                            // Calculate the offset for each sphere in the horizontal direction
                            float horizontalOffset = (k - (spheresPerSet - 1) / 2.0f) * horizontalSpacing;
                            Vector3 horizontalPosition = basePosition + new Vector3(horizontalOffset, -sphereVerticalOffset, 0); // Apply vertical offset

                            // Instantiate the sphere collider
                            GameObject collider = Instantiate(sphereColliderPrefab, horizontalPosition, Quaternion.identity);

                            // Assign the collider to the default layer to interact with the player
                            collider.layer = LayerMask.NameToLayer("Default");
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Not enough energy to create any part of the track.");
        }

        // Clear the recorded positions after creating the colliders
        positions.Clear();
    }

}
