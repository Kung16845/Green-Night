using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombiefirefighter : Zombie
{
    void Awake()
    {
        SetZombieCostumeId();
    }
    public void SetZombieCostumeId()
    {
        string mutationCode = GetMutationCode(mutationType);
        idZombieCoustume = $"30102{mutationCode}";
        Debug.Log($"Zombie Costume ID set to: {idZombieCoustume}");
    }
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
