using UnityEngine;
using Yarn.Unity;



public class GameControl : MonoBehaviour
{
    public DialogueRunner dialogueRunner; // Reference to Yarn DialogueRunner
    public DialogueUI dialogueUI; // Reference to Dialogue UI for displaying options

    void Start()
    {
        // Register the 'options' command
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("options", HandleOptionsCommand);
            Debug.Log("Command handler registered"); // Check if this logs when the game starts
        }
        else
        {
            Debug.LogError("DialogueRunner is null!");
        }
    }

    // Update function to manage dialogue progression
    void Update()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleEKeyAction();
            }
        }
        else
        {
            // Dialogue is running, use E key or Enter to advance dialogue
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
            }
        }
    }

    // Custom 'options' command logic
    void HandleOptionsCommand()
    {
        Debug.Log("HandleOptionsCommand called"); // Add this to check if this function is triggered
        dialogueUI.ShowOptions(); // Display options in the UI
    }

    private void HandleEKeyAction()
    {
        Debug.Log("E key action triggered!");
    }


    // Method for handling Enter key functionality outside dialogue
    private void HandleEnterKeyAction()
    {
        Debug.Log("Enter key action triggered!");
        // Add your specific functionality here
    }

}