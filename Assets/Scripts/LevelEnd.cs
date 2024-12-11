using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Movement movement;

    // Reference to the end game AudioClip
    public AudioClip endGameClip;

    private void Start()
    {
        movement = GameObject.FindWithTag("Player").GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Stop the game time
            Time.timeScale = 0f;

            // Show the end game UI
            UIManager.Instance.ShowEndGamePanel("Heist Successful!");

            // Stop camera scrolling
            CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
            if (cameraScroll != null)
            {
                cameraScroll.StopScrolling();
            }

            // Stop player movement
            if (movement != null)
            {
                movement.moveVelocity = Vector2.zero;
            }

            // Play the end game sound
            if (endGameClip != null)
            {
                AudioSource.PlayClipAtPoint(endGameClip, Camera.main.transform.position);
            }
            else
            {
                Debug.LogWarning("End game clip is not assigned!");
            }
        }
    }
}
