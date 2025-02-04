using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // Reference to the door
    public Vector3 closedPosition; // Position when the door is closed
    public Vector3 openPosition; // Position when the door is open
    public float openDelay = 2f; // Delay time before opening the door
    public bool startsOpen = false; // Set to true if the door is initially open
    public DoorController linkedDoor; // Reference to another door to be closed if this one closes
    public float doorSpeed = 2f; // Speed at which the door moves

    private bool isDoorOpen; // Tracks if the door is currently open
    private bool checkpoint1Triggered = false;
    private bool checkpoint2Triggered = false;
    private Vector3 initialDoorPosition;

    void Start()
    {
        initialDoorPosition = door.position;
        isDoorOpen = startsOpen;

        // Set initial position based on whether the door starts open or closed
        door.position = startsOpen ? initialDoorPosition + openPosition : initialDoorPosition + closedPosition;
    }

    // Called by Lever when the player presses E, only for doors that start closed
    public void ToggleDoor()
    {
        if (!startsOpen && !isDoorOpen) // Ensures only the initially closed door opens with delay
        {
            StartCoroutine(OpenDoorAfterDelay(openDelay));
        }
    }

    private IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveDoor(initialDoorPosition + openPosition));
        isDoorOpen = true;
    }

    public void TriggerCheckpoint(int index)
    {
        if (index == 1)
        {
            checkpoint1Triggered = true;
        }
        else if (index == 2 && checkpoint1Triggered) // Ensure checkpoints are triggered in order
        {
            checkpoint2Triggered = true;
        }

        // Close the door if both checkpoints have been triggered
        if (checkpoint1Triggered && checkpoint2Triggered && startsOpen)
        {
            CloseDoor();
        }
    }

    public void CloseDoor()
    {
        StartCoroutine(MoveDoor(initialDoorPosition + closedPosition));
        isDoorOpen = false;

        // Check if there is a linked door to close as well
        if (linkedDoor != null)
        {
            linkedDoor.CloseDoor();
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition, doorSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
