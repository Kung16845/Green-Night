using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIEDUnit : Zombie
{
    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers();
        damageMultipliers[DamageType.Explosive] = 0.1f;
        damageMultipliers[DamageType.Fire] = 0.9f;
        damageMultipliers[DamageType.HighcalliberBullet] = 0.9f;
        damageMultipliers[DamageType.MediumcaliberBullet] = 0.9f;
        damageMultipliers[DamageType.LowcaliberBullet] = 0.9f;
        damageMultipliers[DamageType.Acid] = 2.5f;
        damageMultipliers[DamageType.Poison] = 1.5f;
    }
    void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
