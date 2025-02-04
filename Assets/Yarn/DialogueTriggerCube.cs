using UnityEngine;
using Yarn.Unity;

public class DialogueTriggerCube : MonoBehaviour
{
    public DialogueRunner dialogueRunner; // Reference to the DialogueRunner component
    public string dialogueNode = "Start"; // Starting node in the Yarn script
    public CanvasGroup dialogueCanvasGroup; // CanvasGroup for controlling dialogue visibility
    private bool playerInRange = false; // Track if player is in range

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueRunner.IsDialogueRunning)
            {
                ShowDialogue(); // Show the dialogue UI when player enters
                dialogueRunner.StartDialogue(dialogueNode); // Start the dialogue
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (!dialogueRunner.IsDialogueRunning)
            {
                HideDialogue(); // Hide the dialogue UI
            }
        }
    }

    private void ShowDialogue()
    {
        dialogueCanvasGroup.alpha = 1f; // Show the dialogue
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;
    }

    private void HideDialogue()
    {
        dialogueCanvasGroup.alpha = 0f; // Hide the dialogue
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        if (playerInRange && !dialogueRunner.IsDialogueRunning)
        {
            HideDialogue(); // Hide dialogue if the player leaves and dialogue is over
        }
    }
}
