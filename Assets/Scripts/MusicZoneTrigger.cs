using UnityEngine;

public class MusicZoneTrigger : MonoBehaviour
{
    public AudioClip zoneMusic; // The music to play when entering this zone
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>(); // Find the AudioManager in the scene
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioManager != null)
        {
            audioManager.PlayMusic(zoneMusic);
        }
    }
}
