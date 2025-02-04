using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleStand : MonoBehaviour
{
    public string marbleTag = "Marble";

    // Start is called before the first frame update
    public bool isOccupied()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(marbleTag))
            {
                return true;
            }
          
        }
        return false;

    }
}
