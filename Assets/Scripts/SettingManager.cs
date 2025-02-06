using UnityEngine.UI;
using UnityEngine;

public class SettingsManager: MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject settingsPanel;        // 
    public TMPro.TMP_Dropdown controlTypeDropdown;    // menu
    public Button settingsButton;           // open setting button
    public Button closeSettingsButton;      // close setting button

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadSettings()
    {
        // load the setting you saved
        int controlType = PlayerPrefs.GetInt("ControlType", 0);
        Debug.Log($"SettingsManager LoadSettings - Control Type: {controlType}");

        if (controlTypeDropdown != null)
        {
            controlTypeDropdown.value = controlType;
            // immediately apply
            SetControlType(controlType);
        }
        else
        {
            Debug.LogError("SettingsManager: controlTypeDropdown is null!");
        }
    }

    public void ForceControllerMode()
    {
        Debug.Log("Forcing Controller Mode");
        SetControlType(1);
        if (controlTypeDropdown != null)
        {
            controlTypeDropdown.value = 1;
        }
    }

    public void SetControlType(int type)
    {
        Debug.Log($"Setting control type to: {type}");
        PlayerPrefs.SetInt("ControlType", type);
        PlayerPrefs.Save();
        Debug.Log($"SettingsManager: Saved control type. Current value in PlayerPrefs: {PlayerPrefs.GetInt("ControlType", 0)}");


        // Find and update all active PlayerMovement scripts
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach (var player in players)
        {
            player.SetControlMode(type == 1);
        }
        /*
        // Inform PlayerMovement update
        var player = GameObject.FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.SetControlMode(type == 1);
        }
        else
        {
            Debug.LogError("SettingsManager: Could not find PlayerMovement component!");
        }
        */
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}