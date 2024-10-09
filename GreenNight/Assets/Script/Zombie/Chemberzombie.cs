using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemberzombie : Zombie
{
    [Header("Speed Boost Settings")]
    public GameObject speedBoostAreaPrefab;
    public float areaRadius = 3f;
    public float areaDuration = 10f;
    public float speedIncreaseAmount = 1.5f;          // Movement speed multiplier
    public float attackSpeedIncreaseAmount = 1.5f;    // Attack speed multiplier
    public float speedIncreaseDuration = 5f; 

    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers(); // Initialize with default multipliers

        // Immune to acid damage
        damageMultipliers[DamageType.Acid] = 0f;
    }

    protected override void OnDeath()
    {
        SpawnSpeedBoostArea();
    }

    private void SpawnSpeedBoostArea()
    {
        if (speedBoostAreaPrefab != null)
        {
            GameObject area = Instantiate(speedBoostAreaPrefab, transform.position, Quaternion.identity);
            SpeedBoostArea areaScript = area.GetComponent<SpeedBoostArea>();
            if (areaScript != null)
            {
                areaScript.Initialize(
                    areaRadius,
                    areaDuration,
                    speedIncreaseAmount,
                    attackSpeedIncreaseAmount, // Pass the attack speed increase
                    speedIncreaseDuration
                );
            }
        }
    }
    private void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
