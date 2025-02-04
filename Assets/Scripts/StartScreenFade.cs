using UnityEngine;
using UnityEngine.UI;

public class StartScreenFade : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Duration of the fade-out effect

    private CanvasGroup canvasGroup;  // Canvas Group for controlling transparency
    private bool isFading = false;    // Flag to check if the screen is already fading

    void Start()
    {
        // Get the Canvas Group component attached to the UI screen (Panel)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // If the Canvas Group component doesn't exist, add one
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Initial setup: ensure the UI screen is fully visible
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true); // Make sure the screen is active at the start
    }

    void Update()
    {
        // Check if the player presses the space key and if the screen is not already fading
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            StartCoroutine(FadeOut());  // Start the fade-out coroutine
        }
    }

    // Coroutine to fade out the UI screen gradually
    private System.Collections.IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        // Gradually reduce the alpha value over the duration of the fade
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;  // Wait for the next frame
        }

        // Ensure the screen is completely invisible
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);  // Deactivate the UI screen once faded out
        isFading = false;

        // Start the game here (e.g., enable player controls, start timers, etc.)
        StartGame();
    }

    // This function will be called to start the actual game logic
    private void StartGame()
    {
        Debug.Log("Game has started!");
        // Add your game start logic here
    }
}
