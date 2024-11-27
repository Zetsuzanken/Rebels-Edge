using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Movement movement;

    [Header("Audio Settings")]
    public AudioSource audioSource;         // AudioSource komponent
    public AudioClip victorySound;          // Heli võidu puhul

    private void Start()
    {
        movement = GameObject.FindWithTag("Player").GetComponent<Movement>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Oletame, et AudioSource on sama objektiga
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Mängi võidu heli
            PlayVictorySound();

            // Lülita aeg välja
            Time.timeScale = 0f;

            // Näita lõpp- mängu paneel
            UIManager.Instance.ShowEndGamePanel("Heist Successful!");

            // Peata kaamera liikumine
            CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
            if (cameraScroll != null)
            {
                cameraScroll.StopScrolling();
            }

            // Lõpeta mängija liikumine
            if (movement != null)
            {
                movement.moveVelocity = Vector2.zero;
            }
        }
    }

    // Mängib võidu heli
    private void PlayVictorySound()
    {
        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
    }
}
