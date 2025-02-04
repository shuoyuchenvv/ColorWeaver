
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialRing : MonoBehaviour
{
    public GameObject panel; // 
    public GameObject campfire;
    public string playerTag = "Player"; // 
    public float displayTime = 5f; // 

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
            // 
            StartCoroutine(HidePanelAfterDelay());
        }
    }

    // 
    IEnumerator HidePanelAfterDelay()
    {
        // 
        yield return new WaitForSeconds(displayTime);
        //
        panel.SetActive(false);
        Destroy(campfire);
        Destroy(gameObject);
        
    }
}






