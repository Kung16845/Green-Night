using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riotshield : Zombie
{
    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers();
        damageMultipliers[DamageType.Fire] = 0.8f;
        damageMultipliers[DamageType.LowcaliberBullet] = 0.5f;
        damageMultipliers[DamageType.Acid] = 1.5f;
    }
    void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
