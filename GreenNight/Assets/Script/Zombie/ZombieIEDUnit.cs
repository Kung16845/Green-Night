using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIEDUnit : Zombie
{
    void Awake()
    {
        SetZombieCostumeId();
    }
    public void SetZombieCostumeId()
    {
        string mutationCode = GetMutationCode(mutationType);
        idZombieCoustume = $"30103{mutationCode}";
    }
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
        if (currentHp <= 0)
        {
            currentState = ZombieState.Dead;
            return;
        }
        if (HasReachedAttackPoint())
        {
            rb2D.velocity = Vector2.zero;
            ZombieAttack();
        }
        else
        {
            ZombieMoveFindBarrier();
        }
    }
}
