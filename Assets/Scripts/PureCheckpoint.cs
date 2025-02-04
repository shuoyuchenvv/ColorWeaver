using UnityEngine;

public class PureCheckpoint: MonoBehaviour
{
    private bool isActivated = false; // To ensure the checkpoint is activated only once
    public PlayerDeathHandler playerDeathHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            ActivateCheckpoint(other.transform.position);
        }
    }

    private void ActivateCheckpoint(Vector3 playerPosition)
    {
        isActivated = true;
        playerDeathHandler.SetCheckpoint(transform.position); // Save the checkpoint globally
        Debug.Log("Checkpoint activated at position: " + playerPosition);
    }
}
