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
        if (controlTypeDropdown != null)
        {
            controlTypeDropdown.value = controlType;
        }
    }

    public void SetControlType(int type)
    {
        PlayerPrefs.SetInt("ControlType", type);
        PlayerPrefs.Save();
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}