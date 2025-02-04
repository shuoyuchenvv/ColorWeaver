using UnityEngine;
using Yarn.Unity;

public class TomatoSpeech: MonoBehaviour
{
    public string dialogueNode = "TomatoIntroduction"; // Node to start when interacting with NPC
    public DialogueRunner dialogueRunner; // Reference to the DialogueRunner
    public CanvasGroup dialogueCanvasGroup; // CanvasGroup for managing dialogue UI visibility

    private bool playerInRange = false; // Track if player is in range of the NPC

    // Triggered when the player enters the NPC's range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Ensure the DialogueRunner is assigned
            if (dialogueRunner == null)
            {
                Debug.LogError("DialogueRunner is null! Please assign it in the Inspector.");
                return;
            }

            // Start the dialogue if it's not already running
            if (!dialogueRunner.IsDialogueRunning)
            {
                ShowDialogue(); // Make the dialogue UI visible
                Debug.Log($"Starting dialogue node: {dialogueNode}");
                dialogueRunner.StartDialogue(dialogueNode); // Start the specified dialogue node
            }
        }
    }

    // Triggered when the player exits the NPC's range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hide the dialogue UI when the player leaves range and no dialogue is running
            if (!dialogueRunner.IsDialogueRunning)
            {
                HideDialogue();
            }
        }
    }

    // Show the dialogue UI
    private void ShowDialogue()
    {
        dialogueCanvasGroup.alpha = 1f; // Make the canvas visible
        dialogueCanvasGroup.interactable = true; // Allow interaction
        dialogueCanvasGroup.blocksRaycasts = true; // Allow raycasting for button clicks
        Debug.Log("Dialogue UI shown.");
    }

    // Hide the dialogue UI
    private void HideDialogue()
    {
        dialogueCanvasGroup.alpha = 0f; // Make the canvas invisible
        dialogueCanvasGroup.interactable = false; // Disable interaction
        dialogueCanvasGroup.blocksRaycasts = false; // Block raycasting to prevent interaction
        Debug.Log("Dialogue UI hidden.");
    }
}
