using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    private Vector3? checkpointPosition = null; // Nullable Vector3 to store checkpoint position
    private bool shouldRespawn = false; // Track if we should respawn
    private Collider playerCollider; // Reference to the player's collider
    private float respawnDelay = 0.1f; // Small delay to avoid immediate collision after respawn
    private float respawnTime = 0f; // Timer to track when to re-enable collider
   
    
    void Start()
    {
        // Get the player's collider at the start
        playerCollider = GetComponent<Collider>();

        // Try to load the checkpoint from the CheckpointManager
        checkpointPosition = CheckpointManager.GetActiveCheckpoint();
        if (checkpointPosition.HasValue)
        {
            Debug.Log("Loaded checkpoint at: " + checkpointPosition.Value);
        }
    }

    private void Die()
    {
        Debug.Log("Die method called");

        if (checkpointPosition.HasValue)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Disable kinematic to allow velocity changes
                rb.isKinematic = false;
                rb.velocity = Vector3.zero; // Reset velocity to stop falling
                rb.angularVelocity = Vector3.zero; // Reset angular velocity
            }

            // Disable the player's collider temporarily to prevent collision on respawn
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
                respawnTime = Time.time + respawnDelay; // Set the delay for re-enabling the collider
            }

            // Set the flag to true so LateUpdate will handle the respawn
            shouldRespawn = true;

            Debug.Log("Player respawned at checkpoint: " + checkpointPosition.Value);
        }
        else
        {
            Debug.Log("No checkpoint activated, reloading scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            Debug.Log("Player touched the fog. Triggering death.");
            Die();
        }
    }

    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        checkpointPosition = newCheckpointPosition;

        // Save the checkpoint globally
        CheckpointManager.ActivateCheckpoint(newCheckpointPosition);

        Debug.Log("Checkpoint set at: " + checkpointPosition.Value);
    }

    private void LateUpdate()
    {
        if (shouldRespawn && checkpointPosition.HasValue)
        {
            // Move player to the checkpoint position
            transform.position = checkpointPosition.Value;
            Debug.Log("Player position set in LateUpdate: " + transform.position);

            shouldRespawn = false; // Reset the flag
        }

        // Re-enable the player's collider after a small delay
        if (!playerCollider.enabled && Time.time >= respawnTime)
        {
            playerCollider.enabled = true;
            Debug.Log("Player collider re-enabled.");
        }
    }
}

