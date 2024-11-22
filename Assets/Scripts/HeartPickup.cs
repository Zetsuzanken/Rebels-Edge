using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthAmount = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healthAmount);

                // Optionally, play a sound effect or animation here

                Destroy(gameObject);
            }
        }
    }
}
