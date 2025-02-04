using UnityEngine;

public class FlowerTrigger : MonoBehaviour
{
    public FlowerManager flowerManager; // Reference to the FlowerManager

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming Player tag is correct
        {
            flowerManager.HandleFlowerTouched(gameObject); // Notify the FlowerManager when the player touches this flower
        }
    }
}
