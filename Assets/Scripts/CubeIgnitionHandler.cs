using UnityEngine;

public class CubeIgnitionHandler : MonoBehaviour
{
    private Ignitable ignitable;  // Reference to the Ignitable script on the same or another object
    public GameObject objectToActivate; // The object that should be activated when the cube is ignited by yellow fire

    void Update()
    {
        ignitable = GetComponent<Ignitable>();
        // 
        if (ignitable != null && ignitable.isIgnited)
        {
            string currentColor = ignitable.colorChangerKeyBoard.GetCurrentColor();

            if (currentColor == "Yellow")
            {
                // 
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                    Debug.Log($"{objectToActivate.name} has been activated because the cube was ignited with yellow fire!");
                }
            }
        }
    }
}

