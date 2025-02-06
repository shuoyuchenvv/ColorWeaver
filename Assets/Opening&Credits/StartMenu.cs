using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu: MonoBehaviour
{
    public GameObject settingsButton;
    public GameObject mainButtons;
    public GameObject settingsPanel;


    /*
    public enum ControlType
    {
        KeyboardMouse,
        Controller
    }*/

    public void SetControlType(int controlType)
    {
        PlayerPrefs.SetInt("ControlType", controlType);
        PlayerPrefs.Save();
    }
    

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

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);      // show setting panel
        mainButtons.SetActive(false);       // hide main menu button
        settingsButton.SetActive(false);    // hide setting button
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);     // hide setting panel
        mainButtons.SetActive(true);        // show main menu button
        settingsButton.SetActive(true);     // show setting button
    }


}

