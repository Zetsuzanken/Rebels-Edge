using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private Movement movement;

    private void Start()
    {
        movement = GameObject.FindWithTag("Player").GetComponent<Movement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;

            UIManager.Instance.ShowEndGamePanel("Heist Successful!");

            CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
            if (cameraScroll != null)
            {
                cameraScroll.StopScrolling();
            }

            if (movement != null)
            {
                movement.moveVelocity = Vector2.zero;
            }
        }
    }
}