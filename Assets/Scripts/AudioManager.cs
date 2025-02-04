using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public float fadeDuration = 1f; // Default fade duration

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play new music with fade-out and fade-in effect.
    /// </summary>
    public void PlayMusic(AudioClip newClip)
    {
        StartCoroutine(FadeToNewMusic(newClip));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        // 1. Fade out the current music
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Stop current music and switch to the new music
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // 2. Fade in the new music
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
