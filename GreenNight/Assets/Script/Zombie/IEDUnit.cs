using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEDUnit : Zombie
{
    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers();
        damageMultipliers[DamageType.Explosive] = 0f;
        damageMultipliers[DamageType.Fire] = 0.9f;
        damageMultipliers[DamageType.HighcalliberBullet] = 0.9f;
        damageMultipliers[DamageType.LowcaliberBullet] = 0.9f;
        damageMultipliers[DamageType.Acid] = 2.5f;
    }
    void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
