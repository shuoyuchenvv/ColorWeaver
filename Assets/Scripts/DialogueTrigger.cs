using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner; // Reference to the Dialogue Runner
    public string startNode = "Start";    // Name of the starting dialogue node
    private bool playerInRange = false;   // Flag to check if the player is in range

    void Update()
    {
        // If the player is in range and presses the "E" key, start the dialogue
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueRunner.StartDialogue(startNode);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Set the flag to true when the player is in range
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Set the flag to false when the player leaves the range
        }
    }
}
