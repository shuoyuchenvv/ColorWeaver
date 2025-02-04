using UnityEngine;

public class RingTrigger : MonoBehaviour
{
    public RingManager ringManager; // Reference to the RingManager

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming Player tag is correct
        {
            // Notify the RingManager to handle the interaction with color check
            ringManager.HandleRingPassed(gameObject);
        }
    }
}
