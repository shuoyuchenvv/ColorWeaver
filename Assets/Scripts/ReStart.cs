using UnityEngine;
using UnityEngine.SceneManagement;  // 
public class ReStart : MonoBehaviour
{
    void Update()
    {
        // 
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed. Restarting game...");
            RestartGame();
        }
    }

    void RestartGame()
    {
        // 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
