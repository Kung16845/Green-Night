using UnityEngine;

public class StatAmplifier : MonoBehaviour
{
    public int endurance = 1; // Increases max stamina
    public int combat = 1;    // Increases stability, accuracy, and decreases reload speed
    public int speed = 1;     // Increases movement speed and stamina regen

    // Method to calculate the percentage increase based on the level
    private float GetAmplifierMultiplier(int level)
    {
        if (level >= 2 && level <= 3) return 0.05f * (level - 1);    // 5% per level
        if (level >= 4 && level <= 6) return 0.10f + 0.07f * (level - 3); // 7% per level
        if (level >= 7 && level <= 9) return 0.31f + 0.04f * (level - 6); // 4% per level
        if (level == 10) return 0.46f + 0.10f;                       // 10% for level 10
        return 0f;
    }

    public float GetEnduranceMultiplier() => 1 + GetAmplifierMultiplier(endurance);
    public float GetCombatMultiplier() => 1 + GetAmplifierMultiplier(combat);
    public float GetSpeedMultiplier() => 1 + GetAmplifierMultiplier(speed);
}
