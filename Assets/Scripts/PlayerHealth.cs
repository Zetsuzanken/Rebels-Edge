using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;     // Maximum health
    public float currentHealth;        // Current health

    [Header("UI Elements")]
    public TextMeshProUGUI healthText; // Reference to HealthText UI

    private bool isDead = false;

    void Start()
    {
        // Initialize health to maximum at the start
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    /// <summary>
    /// Apply damage to the player.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply.</param>
    public void TakeDamage(float damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Instantly kill the player (e.g., falling off a roof).
    /// </summary>
    public void InstantDeath()
    {
        if (isDead)
            return;

        currentHealth = 0f;

        UpdateHealthUI();

        Die();
    }

    /// <summary>
    /// Handles player death.
    /// </summary>
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // TODO: Handle death (play animation, disable controls, etc.)
        Debug.Log("Player has died.");

        // Show the End Game Panel
        UIManager.Instance.ShowEndGamePanel();

        // Optionally, disable player controls here
    }

    /// <summary>
    /// Updates the Health UI text.
    /// </summary>
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + Mathf.RoundToInt(currentHealth).ToString();
        }
    }

    // Testing inputs
    void Update()
    {
        // Simulate taking damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1f); // Adjust damage as needed
            Debug.Log("Player took 1 damage. Current Health: " + currentHealth);
        }

        // Simulate instant death
        if (Input.GetKeyDown(KeyCode.K))
        {
            InstantDeath();
            Debug.Log("Player fell off the roof and died.");
        }
    }
}
