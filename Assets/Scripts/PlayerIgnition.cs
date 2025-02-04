using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform player; // 
    public float doubleClickTimeLimit = 0.3f; //
    private float lastClickTime = 0f; // 
    private Ignitable currentIgnitable; // 

    void Update()
    {
        // 
        DetectIgnitable();

        // 
        if (Input.GetMouseButtonDown(0))
        {
            // 
            if (Time.time - lastClickTime <= doubleClickTimeLimit && currentIgnitable != null)
            {
                // 
                if (currentIgnitable.isIgnited)
                {
                    currentIgnitable.Extinguish();
                }
                // 
                else
                {
                    currentIgnitable.Ignite(); // Ignites the object

                    // If the ignited object is a checkpoint, activate it
                    Checkpoint checkpoint = currentIgnitable.GetComponent<Checkpoint>();
                    if (checkpoint != null)
                    {
                        checkpoint.ActivateCheckpoint(FindObjectOfType<PlayerDeathHandler>());
                    }
                }
            }

            //
            lastClickTime = Time.time;
        }
    }

    // 
    void DetectIgnitable()
    {
        // 
        Ignitable[] ignitables = FindObjectsOfType<Ignitable>();

        foreach (Ignitable ignitable in ignitables)
        {
            if (ignitable.IsInRange(player))
            {
                // 
                currentIgnitable = ignitable;
                return;
            }
        }

        // 
        currentIgnitable = null;
    }
}
