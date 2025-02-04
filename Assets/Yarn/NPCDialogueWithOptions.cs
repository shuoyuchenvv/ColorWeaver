using UnityEngine;
using Yarn.Unity;

public class NPCDialogueWithOptions : MonoBehaviour
{
    public string dialogueNode = "NPCConversation"; // Yarn dialogue node to start from
    public DialogueRunner dialogueRunner; // Reference to Yarn DialogueRunner
    public CanvasGroup dialogueCanvasGroup; // CanvasGroup to control dialogue UI visibility
    public OptionView optionView; // Reference to the OptionView for displaying options
    public GameObject cameraController; // Reference to the camera control script (to disable during dialogue)
    private bool playerInRange = false; // Tracks if the player is within range of the NPC
    private bool isDialoguePaused = false; // Indicates if dialogue is paused for player input

    // Triggered when the player enters the NPC's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueRunner.IsDialogueRunning)
            {
                FreezeCameraAndShowMouse(); // Freeze the camera and show the mouse cursor
                ShowDialogue(); // Show the dialogue UI
                dialogueRunner.StartDialogue(dialogueNode); // Start the Yarn dialogue
            }
        }
    }

    // Triggered when the player exits the NPC's trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop(); // Stop the dialogue if running
                HideDialogue(); // Hide the dialogue UI
                UnfreezeCameraAndHideMouse(); // Unfreeze the camera and hide the mouse cursor
            }
        }
    }

    private void Update()
    {
        // Show options after the NPC finishes talking
        if (playerInRange && !dialogueRunner.IsDialogueRunning && !isDialoguePaused)
        {
            ShowOptions(); // Show player options
            isDialoguePaused = true; // Pause the dialogue to wait for player input
        }
    }

    // Show the dialogue UI
    private void ShowDialogue()
    {
        dialogueCanvasGroup.alpha = 1f;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;

        // Hide the OptionView initially while dialogue progresses
        optionView.gameObject.SetActive(false);
    }

    // Show player options after dialogue ends
    private void ShowOptions()
    {
        optionView.gameObject.SetActive(true); // Show the option buttons
    }

    // Hide the dialogue UI
    private void HideDialogue()
    {
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;

        // Hide the options when dialogue is hidden
        optionView.gameObject.SetActive(false);
    }

    // Freeze the camera and show the mouse cursor
    private void FreezeCameraAndShowMouse()
    {
        cameraController.SetActive(false); // Disable camera control
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Unfreeze the camera and hide the mouse cursor
    private void UnfreezeCameraAndHideMouse()
    {
        cameraController.SetActive(true); // Enable camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
