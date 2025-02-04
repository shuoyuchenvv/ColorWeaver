using UnityEngine;

public class AudioCheckpoint : MonoBehaviour
{
    public AudioClip checkpointMusic; // Music to play when this checkpoint is reached

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player triggering the checkpoint
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.PlayMusic(checkpointMusic);
            }
        }
    }
}
