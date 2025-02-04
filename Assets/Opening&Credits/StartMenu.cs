using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu: MonoBehaviour
{
    public void PlayGame()
    {
        
        // "MainScene"
        SceneManager.LoadScene("Suprise");
    }

    public void OpenCredits()
    {
        
        //  "CreditScene"
        SceneManager.LoadScene("CreditScene");
    }
}

