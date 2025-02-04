using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [System.Serializable]
    public class Fan
    {
        public GameObject fanObject; // Fan object
        public float rotationSpeed; // Speed of the fan's rotation
        public Vector3 rotationAxis = Vector3.right; // Axis of rotation
        public float startDelay; // Delay before starting the fan
        [HideInInspector] public Transform fanPivot; // Pivot for the fan
        [HideInInspector] public bool isRotating; // Whether the fan is rotating
    }

    [System.Serializable]
    public class ObjectGroup
    {
        public GameObject[] objects; // Array of objects to change material
        public Material newMaterial; // New material to assign
        public float startDelay; // Delay before applying the material
    }

    public DoorController doorController; // Reference to the door controller
    public Fan[] fans; // Array of fans
    public ObjectGroup[] objectGroups; // Array of object groups
    public Collider checkpointCollider; // Collider representing the checkpoint area

    private bool isPlayerNearby = false; // Check if player is within range

    void Update()
    {
        // Rotate all fans that are marked as rotating
        foreach (var fan in fans)
        {
            if (fan.isRotating && fan.fanPivot != null)
            {
                fan.fanPivot.Rotate(fan.rotationAxis, fan.rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        // Check if player is nearby and presses E key to trigger mechanism
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TriggerMechanism();
        }
    }

    private void TriggerMechanism()
    {
        foreach (var group in objectGroups)
        {
            StartCoroutine(ChangeObjectMaterials(group));
        }

        foreach (var fan in fans)
        {
            StartCoroutine(StartFanRotation(fan));
        }

        doorController.ToggleDoor();
    }

    private IEnumerator ChangeObjectMaterials(ObjectGroup group)
    {
        yield return new WaitForSeconds(group.startDelay);

        foreach (var obj in group.objects)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = group.newMaterial;
            }
        }
    }

    private IEnumerator StartFanRotation(Fan fan)
    {
        yield return new WaitForSeconds(fan.startDelay);

        if (fan.fanObject == null)
        {
            Debug.LogError("Fan object is not assigned.");
            yield break;
        }

        Renderer fanRenderer = FindRenderer(fan.fanObject);
        if (fanRenderer == null)
        {
            Debug.LogError($"No Renderer found on or under the object: {fan.fanObject.name}");
            yield break;
        }

        if (fan.fanPivot == null)
        {
            fan.fanPivot = new GameObject($"{fan.fanObject.name}_Pivot").transform;
            fan.fanPivot.position = fanRenderer.bounds.center;
            fan.fanObject.transform.SetParent(fan.fanPivot);
        }

        Debug.Log($"Starting fan rotation: {fan.fanObject.name}, Speed: {fan.rotationSpeed}, Axis: {fan.rotationAxis}");
        fan.isRotating = true;
    }

    private Renderer FindRenderer(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null) return renderer;

        renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null) return renderer;

        Debug.LogWarning($"Renderer not directly found on {obj.name}. Checking nested children...");

        foreach (Transform child in obj.transform)
        {
            renderer = FindRenderer(child.gameObject);
            if (renderer != null) return renderer;
        }

        return null; // No renderer found
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone."); // Debug log for entering the zone
            isPlayerNearby = true;
        }

        // Check if the player enters the checkpoint
        if (checkpointCollider != null && other == checkpointCollider)
        {
            Debug.Log("Player entered the checkpoint, disabling fans permanently."); // Debug log for checkpoint
            foreach (var fan in fans)
            {
                DisableFan(fan);
            }
        }
        else
        {
            Debug.LogWarning("Checkpoint collider is not set or the triggering object is not the checkpoint."); // Debug warning for setup issues
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Stop rotating the fans when the player leaves the area
            foreach (var fan in fans)
            {
                fan.isRotating = false;
                Debug.Log($"Player left the area, stopping fan: {fan.fanObject.name}"); // Debug log for stopping the fan
            }
        }
    }

    private void DisableFan(Fan fan)
    {
        if (fan.fanObject != null)
        {
            Debug.Log($"Disabling fan: {fan.fanObject.name}"); // Debug log for disabling a specific fan
            fan.isRotating = false; // Stop the rotation logic
            fan.fanObject.SetActive(false); // Disable the fan's parent node
        }
        else
        {
            Debug.LogWarning("Fan object is null, cannot disable."); // Warning for null fan object
        }
    }
}
