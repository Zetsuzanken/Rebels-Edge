using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthAmount = 25f;

    [Header("Audio")]
    public AudioClip powerUpClip; // Reference to the power-up sound
    public float powerUpClipVolume = 1.0f; // Volume for the power-up sound

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healthAmount);

                // Play power-up sound effect
                if (powerUpClip != null)
                {
                    AudioSource.PlayClipAtPoint(powerUpClip, transform.position, powerUpClipVolume);
                }
                else
                {
                    Debug.LogWarning("Power-up audio clip not assigned!");
                }

                // Destroy the health booster object
                Destroy(gameObject);
            }
        }
    }
}
