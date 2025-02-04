using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheck : MonoBehaviour
{
    public GameObject colorPanel;
    public GameObject firepathPanel;


    private void OnTriggerEnter(Collider other)
    {
        

     
        
        if (other.CompareTag("Player"))
        {
            colorPanel.SetActive(true);
            firepathPanel.SetActive(true);
        }
    }
}