using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombiefirefighter : Zombie
{
    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers(); // Initialize with default multipliers

        // Set Fire damage multiplier to 0 (immune to fire)
        damageMultipliers[DamageType.Fire] = 0f;
    }
    void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
