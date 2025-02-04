using UnityEngine;
using Yarn.Unity;

public class HideDialogueAfterComplete : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Reference to CanvasGroup for controlling visibility

    private void Start()
    {
        // Subscribe to the onDialogueComplete event
        DialogueRunner dialogueRunner = GetComponent<DialogueRunner>();
        if (dialogueRunner != null)
        {
            dialogueRunner.onDialogueComplete.AddListener(HideDialogue); // Hide UI after dialogue ends
        }
    }

    // This method will hide the dialogue after it is complete
    public void HideDialogue()
    {
        canvasGroup.alpha = 0f;  // Hide the dialogue by setting alpha to 0
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // This method will show the dialogue at the start
    public void ShowDialogue()
    {
        canvasGroup.alpha = 1f;  // Show the dialogue by setting alpha to 1
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
