using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip uiSelectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = FindObjectOfType<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found in the scene. Please add one!");
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && uiSelectSound != null)
        {
            audioSource.PlayOneShot(uiSelectSound);
        }
    }
}
