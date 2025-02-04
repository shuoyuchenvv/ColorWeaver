using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenActiveCheck : MonoBehaviour
{
    public GameObject TheMarbleStand;
    public GameObject GreenNPC;
    public GameObject endTrigger;
    public GameObject speechTrigger;
    // Start is called before the first frame update
   // public ColorChangerKeyBoard colorChangerKeyBoard;
    //public Ignitable ignitable;

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in TheMarbleStand.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Marble"))
            {
                endTrigger.SetActive(true);
                GreenNPC.SetActive(true);
                speechTrigger.SetActive(true);
            }
        }
    }
}
