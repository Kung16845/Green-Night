using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float barrierDamage;
    private float zombieDamage;
    private float radius;

    public void Initialize(float barrierDamage, float zombieDamage, float radius)
    {
        this.barrierDamage = barrierDamage;
        this.zombieDamage = zombieDamage;
        this.radius = radius;

        ApplyDamage();
        Destroy(this.gameObject); // Destroy after applying damage
    }

    private void ApplyDamage()
    {
        // Apply damage to barriers and zombies within radius
        // (Implement collision detection and damage application here)
    }
}
