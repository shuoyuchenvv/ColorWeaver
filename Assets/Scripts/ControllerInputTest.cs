using UnityEngine;
using TMPro;

public class ControllerInputTest : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    void Update()
    {
        string debugInfo = "PS5 Controller Test:\n\n";

        //
        debugInfo += "Configured Axes:\n";
        debugInfo += $"Right_Horizontal: {Input.GetAxisRaw("Right_Horizontal"):F2}\n";
        debugInfo += $"Right_Vertical: {Input.GetAxisRaw("Right_Vertical"):F2}\n";
        debugInfo += $"DPad_Horizontal: {Input.GetAxisRaw("DPad_Horizontal"):F2}\n";
        debugInfo += $"DPad_Vertical: {Input.GetAxisRaw("DPad_Vertical"):F2}\n";

        // 
        debugInfo += "\nButtons:\n";
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0 + i))
            {
                debugInfo += $"Button {i} pressed\n";
            }
        }

        // 
        string[] joysticks = Input.GetJoystickNames();
        debugInfo += $"\nConnected Controllers: {joysticks.Length}\n";
        for (int i = 0; i < joysticks.Length; i++)
        {
            debugInfo += $"Controller {i}: {joysticks[i]}\n";
        }

        debugText.text = debugInfo;
    }
}