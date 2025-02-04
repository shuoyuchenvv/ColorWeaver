using UnityEngine;

public class PanelDeactivator : MonoBehaviour
{
    // 
    private void OnTriggerEnter(Collider other)
    {
        // 
        if (other.CompareTag("Player"))
        {
            // 
            gameObject.SetActive(false);
        }
    }
}
