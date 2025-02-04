using UnityEngine;

public class TutorialFirePath : MonoBehaviour
{
 
    public GameObject prefabnext;  // 
    

    public GameObject uiPanelcurrent;  //  

   

    private void OnTriggerEnter(Collider other)
    {
        // 
        Debug.Log("collide");
        if (other.CompareTag("Player"))
        {
            
            uiPanelcurrent.SetActive(true);  // 
        }
    }

    private void Update()
    {
        // 
        if (Input.GetMouseButtonDown(1))
        {
            uiPanelcurrent.SetActive(false);  // 
            prefabnext.SetActive(true);       // 

            
            Invoke("DeactivateSelf", 0.1f);  // 
        }
    }

    void DeactivateSelf()
    {
        gameObject.SetActive(false);  // 
    }




}
