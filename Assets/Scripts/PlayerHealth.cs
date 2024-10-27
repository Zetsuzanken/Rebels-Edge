using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float lerpSpeed = 0.01f;

    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateHealthUI();
        HandleInput();
    }

    /// <summary>
    /// Apply damage to the player.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply.</param>
    public void TakeDamage(float damageAmount)
    {
        anim.SetBool("IsDead", true);
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

        anim.SetBool("IsDead", isDead);

        CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
        if (cameraScroll != null)
        {
            cameraScroll.StopScrolling();
        }

        StartCoroutine(ShowEndGamePanelAfterDelay());

        // Optionally, disable player controls here
    }

    private IEnumerator ShowEndGamePanelAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        UIManager.Instance.ShowEndGamePanel("Game Over!");
    }

    /// <summary>
    /// Updates the Health UI.
    /// </summary>
    private void UpdateHealthUI()
    {
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
    }

    /// <summary>
    /// Handles input for testing purposes.
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            InstantDeath();
        }
    }
}