using UnityEngine;
using TMPro;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;             // Maximum energy
    public float currentEnergy;                // Current energy
    public float energyRegenRate = 5f;         // Energy regenerated per second

    [Header("Energy Costs")]
    public float sprintEnergyCostPerSecond = 10f;  // Energy cost per second while sprinting
    public float meleeAttackEnergyCost = 15f;      // Energy cost per melee attack

    [Header("UI Elements")]
    public TextMeshProUGUI energyText;              // Reference to EnergyText UI

    private bool isSprinting = false;

    void Start()
    {
        // Initialize energy to maximum at the start
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
    }

    void Update()
    {
        HandleEnergyRegeneration();
        HandleInput(); // For testing purposes
    }

    /// <summary>
    /// Regenerates energy over time when not sprinting or attacking.
    /// </summary>
    void HandleEnergyRegeneration()
    {
        if (!isSprinting)
        {
            // Regenerate energy over time
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
    /// Handles input for testing sprinting and attacking.
    /// </summary>
    void HandleInput()
    {
        // Simulate sprinting when holding Left Shift
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
                Debug.Log("Out of energy! Cannot sprint.");
            }
        }
        else
        {
            isSprinting = false;
            // TODO: Reset player speed to normal
        }

        // Simulate melee attack when pressing Left Mouse Button
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentEnergy >= meleeAttackEnergyCost)
            {
                UseEnergy(meleeAttackEnergyCost);
                // TODO: Execute melee attack
                Debug.Log("Performed melee attack. Current Energy: " + currentEnergy);
            }
            else
            {
                Debug.Log("Not enough energy to perform melee attack.");
            }
        }
    }

    /// <summary>
    /// Updates the Energy UI text.
    /// </summary>
    private void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energy: " + Mathf.RoundToInt(currentEnergy).ToString();
        }
    }
}
