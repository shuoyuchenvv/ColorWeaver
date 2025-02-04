using UnityEngine;
using Yarn.Unity;

public class NPCDialogueOnly : MonoBehaviour
{
    // Dialogue Nudes
    public string defaultDialogueNode = "BirdTeach"; // Default dialogue node
    public string yellowFireDialogueNode = "BirdTeachYellow"; // Node for yellow fire dialogue
    public string blueFireDialogueNode = "BirdTeachBlue"; // Node for blue fire dialogue
    public string otherFireDialogueNode = "BirdTeachGreenOrRed"; // Node for red/green fire dialogue
    public DialogueRunner dialogueRunner; // Reference to the DialogueRunner
    public CanvasGroup dialogueCanvasGroup; // CanvasGroup for managing dialogue panel visibility
    public Ignitable ignitableObject; // Reference to the Ignitable object (controls fire state)

    private bool playerInRange = false; // Track if player is in range

    // Triggered when the player enters the NPC's range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Ensure required components are assigned
            if (ignitableObject == null)
            {
                Debug.LogError("ignitableObject is null! Please assign it in the Inspector.");
            }

            if (dialogueRunner == null)
            {
                Debug.LogError("dialogueRunner is null! Please assign it in the Inspector.");
            }

            // Start dialogue only if it's not running
            if (ignitableObject != null && dialogueRunner != null && !dialogueRunner.IsDialogueRunning)
            {
                ShowDialogue(); // Show the dialogue UI

                // Start the appropriate dialogue node based on the ignited state
                if (ignitableObject.isYellowIgnited)
                {
                    Debug.Log("Starting yellow fire dialogue.");
                    dialogueRunner.StartDialogue(yellowFireDialogueNode);
                }
                else if (ignitableObject.isBlueIgnited)
                {
                    Debug.Log("Starting blue fire dialogue.");
                    dialogueRunner.StartDialogue(blueFireDialogueNode);
                }
                else if (ignitableObject.isRedIgnited || ignitableObject.isGreenIgnited)
                {
                    Debug.Log("Starting red/green fire dialogue.");
                    dialogueRunner.StartDialogue(otherFireDialogueNode);
                }
                else
                {
                    Debug.Log("Starting default dialogue.");
                    dialogueRunner.StartDialogue(defaultDialogueNode);
                }
            }
        }
    }

    // Triggered when the player exits the NPC's range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hide the dialogue UI when the player leaves range and the dialogue is not running
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
        dialogueCanvasGroup.interactable = true; // Enable interaction
        dialogueCanvasGroup.blocksRaycasts = true; // Allow raycasting (interaction detection)
        Debug.Log("Dialogue UI shown.");
    }

    // Hide the dialogue UI
    private void HideDialogue()
    {
        dialogueCanvasGroup.alpha = 0f; // Make the canvas invisible
        dialogueCanvasGroup.interactable = false; // Disable interaction
        dialogueCanvasGroup.blocksRaycasts = false; // Block raycasting
        Debug.Log("Dialogue UI hidden.");
    }
}
