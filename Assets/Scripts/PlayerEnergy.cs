using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float energyRegenRate = 5f;

    [Header("Energy Costs")]
    public float sprintEnergyCostPerSecond = 10f;
    public float meleeAttackEnergyCost = 15f;

    [Header("UI Elements")]
    public Slider energySlider;
    public Slider easeEnergySlider;
    public float lerpSpeed = 0.01f;

    private bool isSprinting = false;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
    }

    void Update()
    {
        UpdateEnergyUI();
        HandleEnergyRegeneration();
        HandleInput();
    }

    /// <summary>
    /// Regenerates energy over time when not sprinting or attacking.
    /// </summary>
    void HandleEnergyRegeneration()
    {
        if (!isSprinting)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
            UpdateEnergyUI();
        }
    }

    /// <summary>
    /// Consumes energy for various actions.
    /// </summary>
    /// <param name="amount">Amount of energy to consume.</param>
    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateEnergyUI();
    }

    /// <summary>
    /// Updates the Energy UI.
    /// </summary>
    private void UpdateEnergyUI()
    {
        if (energySlider.value != currentEnergy)
        {
            energySlider.value = currentEnergy;
        }

        if (energySlider.value != easeEnergySlider.value)
        {
            easeEnergySlider.value = Mathf.Lerp(easeEnergySlider.value, currentEnergy, lerpSpeed);
        }
    }

    /// <summary>
    /// Handles input for testing sprinting and attacking.
    /// </summary>
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;

            if (currentEnergy > 0f)
            {
                UseEnergy(sprintEnergyCostPerSecond * Time.deltaTime);
                // TODO: Increase player speed
            }
            else
            {
                // TODO: Handle out-of-energy state (e.g., stop sprinting)
            }
        }
        else
        {
            isSprinting = false;
            // TODO: Reset player speed to normal
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentEnergy >= meleeAttackEnergyCost)
            {
                UseEnergy(meleeAttackEnergyCost);
                // TODO: Execute melee attack
            }
        }
    }
}
