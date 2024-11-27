using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Movement movement;

    [Header("Audio Settings")]
    public AudioSource audioSource;         // AudioSource komponent
    public AudioClip victorySound;          // Heli v�idu puhul

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
            // M�ngi v�idu heli
            PlayVictorySound();

            // L�lita aeg v�lja
            Time.timeScale = 0f;

            // N�ita l�pp- m�ngu paneel
            UIManager.Instance.ShowEndGamePanel("Heist Successful!");

            // Peata kaamera liikumine
            CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
            if (cameraScroll != null)
            {
                cameraScroll.StopScrolling();
            }

            // L�peta m�ngija liikumine
            if (movement != null)
            {
                movement.moveVelocity = Vector2.zero;
            }
        }
    }

    // M�ngib v�idu heli
    private void PlayVictorySound()
    {
        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
    }
}
