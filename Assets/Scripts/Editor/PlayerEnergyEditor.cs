using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerEnergy))]
public class PlayerEnergyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Reference to the PlayerEnergy script
        PlayerEnergy playerEnergy = (PlayerEnergy)target;

        // Header: Energy Settings
        GUILayout.Label("Energy Settings", EditorStyles.boldLabel);
        playerEnergy.maxEnergy = EditorGUILayout.FloatField("Max Energy", playerEnergy.maxEnergy);
        playerEnergy.currentEnergy = EditorGUILayout.FloatField("Current Energy", playerEnergy.currentEnergy);
        playerEnergy.energyRegenRate = EditorGUILayout.FloatField("Energy Regen Rate", playerEnergy.energyRegenRate);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Energy to Max"))
        {
            playerEnergy.currentEnergy = playerEnergy.maxEnergy;
        }
        if (GUILayout.Button("Set Energy to Min"))
        {
            playerEnergy.currentEnergy = 0;
        }
        GUILayout.EndHorizontal();

        // Header: Energy Costs
        GUILayout.Label("Energy Costs", EditorStyles.boldLabel);
        playerEnergy.sprintEnergyCostPerSecond = EditorGUILayout.FloatField("Sprint Energy Cost (Per Sec)", playerEnergy.sprintEnergyCostPerSecond);
        playerEnergy.meleeAttackEnergyCost = EditorGUILayout.FloatField("Melee Attack Energy Cost", playerEnergy.meleeAttackEnergyCost);

        if (GUILayout.Button("Halve All Energy Costs"))
        {
            playerEnergy.sprintEnergyCostPerSecond /= 2f;
            playerEnergy.meleeAttackEnergyCost /= 2f;
        }

        // Header: UI Elements
        GUILayout.Label("UI Elements", EditorStyles.boldLabel);
        playerEnergy.energySlider = (UnityEngine.UI.Slider)EditorGUILayout.ObjectField("Energy Slider", playerEnergy.energySlider, typeof(UnityEngine.UI.Slider), true);
        playerEnergy.easeEnergySlider = (UnityEngine.UI.Slider)EditorGUILayout.ObjectField("Ease Energy Slider", playerEnergy.easeEnergySlider, typeof(UnityEngine.UI.Slider), true);

        GUILayout.Label("Lerp Speed", EditorStyles.boldLabel);
        playerEnergy.lerpSpeed = EditorGUILayout.Slider(playerEnergy.lerpSpeed, 0.01f, 1f);

        // Apply changes if any property is modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(playerEnergy);
        }
    }
}
