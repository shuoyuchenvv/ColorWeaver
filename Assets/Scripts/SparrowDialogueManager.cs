using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Singleton instance
    public TextMeshProUGUI dialogueText; // The TMP text component to show the dialogue

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to start displaying the dialogue
    public void StartDialogue(string dialogue)
    {
        dialogueText.text = dialogue; // Set the dialogue text
    }

    // Method to clear the dialogue
    public void EndDialogue()
    {
        dialogueText.text = ""; // Clear the dialogue text
    }
}
