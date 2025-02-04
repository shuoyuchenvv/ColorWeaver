using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public CanvasGroup optionsCanvasGroup; // Reference to the CanvasGroup for managing visibility
    public Button[] optionButtons; // Array of buttons for dialogue options

    // This method is called to show the dialogue options
    public void ShowOptions()
    {
        Debug.Log("ShowOptions called");
        optionsCanvasGroup.alpha = 1f;
        optionsCanvasGroup.interactable = true;
        optionsCanvasGroup.blocksRaycasts = true;

        foreach (Button optionButton in optionButtons)
        {
            optionButton.gameObject.SetActive(true);
        }
    }


    // This method is called to hide the dialogue options
    public void HideOptions()
    {
        optionsCanvasGroup.alpha = 0f; // Hide the options by setting alpha to 0 (fully invisible)
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;

        // Disable the buttons to hide them
        foreach (Button optionButton in optionButtons)
        {
            optionButton.gameObject.SetActive(false);
        }
    }

    // Called when a player selects an option
    public void SelectOption(int optionIndex)
    {
        // Process the selected option here, for now, just log it
        Debug.Log($"Option {optionIndex} selected.");

        // Hide options after selection
        HideOptions();
    }
}
