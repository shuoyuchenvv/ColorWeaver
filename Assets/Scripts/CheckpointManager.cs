using UnityEngine;

public class CheckpointManager
{
    private static Vector3? activeCheckpoint;

    void Awake()
    {
        activeCheckpoint = null; // 确保清空旧的检查点
    }

    public static void ActivateCheckpoint(Vector3 position)
    {
        activeCheckpoint = position;

        // Save checkpoint position for persistence
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetFloat("CheckpointZ", position.z);
        PlayerPrefs.Save();

        Debug.Log("Checkpoint saved globally at: " + position);
    }

    public static Vector3? GetActiveCheckpoint()
    {
        if (activeCheckpoint == null)
        {
            // Try to load checkpoint from PlayerPrefs
            if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY") && PlayerPrefs.HasKey("CheckpointZ"))
            {
                float x = PlayerPrefs.GetFloat("CheckpointX");
                float y = PlayerPrefs.GetFloat("CheckpointY");
                float z = PlayerPrefs.GetFloat("CheckpointZ");
                activeCheckpoint = new Vector3(x, y, z);
            }
        }
        return activeCheckpoint;
    }
}
