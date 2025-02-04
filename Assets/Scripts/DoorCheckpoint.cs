using UnityEngine;

public class DoorCheckpoint : MonoBehaviour
{
    public DoorController doorController;
    public int checkpointIndex; // 1 for Checkpoint1, 2 for Checkpoint2

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorController.TriggerCheckpoint(checkpointIndex);
            this.gameObject.SetActive(false); // Disable checkpoint after triggering
        }
    }
}
