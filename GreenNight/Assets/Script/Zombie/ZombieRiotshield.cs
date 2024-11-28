using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRiotshield : Zombie
{
    void Awake()
    {
        SetZombieCostumeId();
    }
    public void SetZombieCostumeId()
    {
        string mutationCode = GetMutationCode(mutationType);
        idZombieCoustume = $"30109{mutationCode}";
        Debug.Log($"Zombie Costume ID set to: {idZombieCoustume}");
    }
    protected override void InitializeDamageMultipliers()
    {
        base.InitializeDamageMultipliers();
        damageMultipliers[DamageType.Fire] = 0.8f;
        damageMultipliers[DamageType.LowcaliberBullet] = 0.25f;
        damageMultipliers[DamageType.Acid] = 1.5f;
    }
    void Update()
    {
        ZombieAttack();
        ZombieMoveFindBarrier();
    }
}
