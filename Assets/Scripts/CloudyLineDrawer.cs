using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyLineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float drawCostPerSecond = 10f;
    public float segmentSpacing = 0.5f;
    public GameObject sphereColliderPrefab; // Prefab of a sphere collider
    public Vector3 offset = new Vector3(0, -1, 0); // Offset to draw the line from the bottom of the player

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
        if (Input.GetKeyDown(KeyCode.Return) && energyBar.UseEnergy(0)) // Check if there's some energy to start
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
        // Only record positions if enough energy is available
        if (energyBar.UseEnergy(drawCostPerSecond * Time.deltaTime))
        {
            Vector3 currentPosition = transform.position + offset; // Adjust position with offset
            if (Vector3.Distance(positions[positions.Count - 1], currentPosition) > segmentSpacing)
            {
                positions.Add(currentPosition);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
            }
        }
        else
        {
            CreateCloudyLine(); // Automatically create the line if energy runs out
        }
    }

    /* single layer of cloud
    void CreateCloudyLine()
    {
        isRecording = false;

        // Instantiate colliders along the recorded positions
        foreach (Vector3 position in positions)
        {
            GameObject collider = Instantiate(sphereColliderPrefab, position, Quaternion.identity);

            // Assign the collider to the default layer to interact with the player
            collider.layer = LayerMask.NameToLayer("Default");
        }

        // Clear the recorded positions after creating the colliders
        positions.Clear();
    }
    */

    // multiply layers of cloud
    void CreateCloudyLine()
    {
        isRecording = false;
        float trackWidth = 2.0f; // Total width of the track
        int numCollidersWide = 3; // Number of colliders to create across the width
        float spacing = trackWidth / (numCollidersWide - 1); // Spacing between colliders horizontally

        // Instantiate colliders along the recorded positions
        foreach (Vector3 position in positions)
        {
            // Create colliders across the horizontal width
            for (int i = 0; i < numCollidersWide; i++)
            {
                // Calculate the offset for each collider in horizontal width
                Vector3 offset = new Vector3((i - (numCollidersWide - 1) / 2.0f) * spacing, 0, 0);
                GameObject collider = Instantiate(sphereColliderPrefab, position + offset, Quaternion.identity);

                // Assign the collider to the default layer to interact with the player
                collider.layer = LayerMask.NameToLayer("Default");
            }
        }

        // Clear the recorded positions after creating the colliders
        positions.Clear();
    }



}
