using UnityEngine;

public class BirdInteraction : MonoBehaviour
{
    public GameObject dialogueUI; // Reference to the dialogue UI
    public string birdDialogue = "Hello, I am the magical bird. You've passed all the rings!";
    private bool hasTriggered = false; // Ensure the dialogue only triggers once

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            TriggerDialogue();
        }
    }

    // Function to trigger the dialogue
    void TriggerDialogue()
    {
        Debug.Log("Bird Dialogue Triggered");
        dialogueUI.SetActive(true); // Activate the dialogue UI
        DialogueManager.Instance.StartDialogue(birdDialogue); // Call the Dialogue Manager to display text
    }
}
