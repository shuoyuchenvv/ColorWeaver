using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated; // To ensure the checkpoint is only activated once

    void Start()
    {
        isActivated = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            // Activate the checkpoint when the player enters the trigger zone
            ActivateCheckpoint(other.GetComponent<PlayerDeathHandler>());
        }
    }

    public void ActivateCheckpoint(PlayerDeathHandler playerDeathHandler)
    {
        if (!isActivated)
        {
            isActivated = true;
            playerDeathHandler.SetCheckpoint(transform.position); // Save the checkpoint position
            Debug.Log("Checkpoint activated at position: " + transform.position);

            // Optionally, change the object's appearance to show it has been activated
            //GetComponent<Renderer>().material.color = Color.green; // Change color to indicate activation
        }
    }
}
