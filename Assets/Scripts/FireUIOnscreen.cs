using UnityEngine;
using UnityEngine.UI;

public class FireUIOnscreen : MonoBehaviour
{
    public GameObject panel; // 
    public string playerTag = "Player"; //

    void Start()
    {
        // 
        panel.SetActive(false);
    }

    // 
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == playerTag)
        {
            // 
            panel.SetActive(true);
        }
    }
}
