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

    private PlayerEnergy playerEnergy;

    [Header("Audio Settings")]
    public AudioSource audioSource;         // AudioSource komponent
    public AudioClip damageSound;           // Heli vigastuse jaoks
    public AudioClip deathSound;            // Heli surma jaoks

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        anim = GetComponent<Animator>();

        playerEnergy = GetComponent<PlayerEnergy>();
        if (playerEnergy == null)
        {
            Debug.LogError("PlayerEnergy component not found on the player!");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Oletame, et AudioSource on sama objektiga
        }
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
        if (isDead)
            return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();

        // Mängi vigastuse heli
        PlayDamageSound();

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

        anim.SetBool("IsDead", isDead);

        // Mängi surma heli
        PlayDeathSound();

        if (playerEnergy != null)
        {
            playerEnergy.SetEnergyToZero();
        }

        CameraScroll cameraScroll = Camera.main.GetComponent<CameraScroll>();
        if (cameraScroll != null)
        {
            cameraScroll.StopScrolling();
        }

        StartCoroutine(ShowEndGamePanelAfterDelay());
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

    public bool IsDead()
    {
        return isDead;
    }

    public void OnDeathAnimationComplete()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowEndGamePanel("Game Over!");
    }

    public void RestoreHealth(float amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    // Mängib vigastuse heli
    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    // Mängib surma heli
    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
