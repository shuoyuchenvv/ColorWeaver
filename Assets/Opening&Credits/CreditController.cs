using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using Newtonsoft.Json; // Ensure Newtonsoft.Json package is installed
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour
{

    [System.Serializable]
    public class CreditEntry
    {
        public string role; // The role or position
        public List<string> names; // List of names for this role
    }

    [System.Serializable]
    public class CreditPage
    {
        public List<CreditEntry> credits; // List of credit entries for this page
    }

    [System.Serializable]
    public class CreditData
    {
        public List<CreditPage> pages; // List of all pages
    }

    public TextAsset jsonFile; // Reference to the JSON file

    // UI references for the three positions
    public TextMeshProUGUI roleText1, namesText1;
    public TextMeshProUGUI roleText2, namesText2;
    public TextMeshProUGUI roleText3, namesText3;

    public float fadeDuration = 2.0f; // Fade in/out duration
    public float displayDuration = 3.0f; // Time to display each page

    private CreditData creditData; // Parsed credit data
    private int currentPageIndex = 0; // Current page index
    private CanvasGroup canvasGroup; // For fade in/out effect

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is missing!");
            return;
        }

        // Load credits from JSON file
        LoadCredits();

        // Start displaying credits if data is loaded
        if (creditData.pages.Count > 0)
        {
            StartCoroutine(DisplayCredits());
        }
        else
        {
            Debug.LogError("No credits found in the JSON file!");
        }
    }

    void LoadCredits()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned!");
            return;
        }

        // Parse the JSON content
        creditData = JsonConvert.DeserializeObject<CreditData>(jsonFile.text);
    }

    IEnumerator DisplayCredits()
    {
        while (currentPageIndex < creditData.pages.Count)
        {
            // Get the current page
            CreditPage currentPage = creditData.pages[currentPageIndex];

            // Display data for each position
            for (int i = 0; i < 3; i++) // Assuming max 3 positions per page
            {
                if (i < currentPage.credits.Count)
                {
                    CreditEntry entry = currentPage.credits[i];
                    SetPositionText(i, entry.role, entry.names);
                }
                else
                {
                    // Clear the unused positions
                    SetPositionText(i, "", new List<string>());
                }
            }

            // Fade in the page
            yield return StartCoroutine(FadeCanvas(0, 1));

            // Display the page for the specified duration
            yield return new WaitForSeconds(displayDuration);

            // Fade out the page
            yield return StartCoroutine(FadeCanvas(1, 0));

            // Move to the next page
            currentPageIndex++;
        }

        Debug.Log("Credits finished!");
        SceneManager.LoadScene("Opening");
    }

    void SetPositionText(int positionIndex, string role, List<string> names)
    {
        TextMeshProUGUI roleText = null;
        TextMeshProUGUI namesText = null;

        switch (positionIndex)
        {
            case 0:
                roleText = roleText1;
                namesText = namesText1;
                break;
            case 1:
                roleText = roleText2;
                namesText = namesText2;
                break;
            case 2:
                roleText = roleText3;
                namesText = namesText3;
                break;
        }

        if (roleText != null && namesText != null)
        {
            roleText.text = role;
            namesText.text = string.Join("\n", names); // Join names with line breaks
        }
    }

    IEnumerator FadeCanvas(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
