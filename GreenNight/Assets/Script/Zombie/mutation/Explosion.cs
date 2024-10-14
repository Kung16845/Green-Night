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
        this.transform.localScale = new Vector3(radius * 2, radius * 2, 1f);
        StartCoroutine(CreateArea());
    }
    private IEnumerator CreateArea()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void ApplyDamage()
    {
        // Apply damage to barriers and zombies within radius
        // (Implement collision detection and damage application here)
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.ZombieTakeDamage(zombieDamage, DamageType.Explosive);
        }

        Barrier barrierComponent = other.GetComponent<Barrier>();
        if (barrierComponent != null)
        {
            barrierComponent.BarrierTakeDamage(barrierDamage);
        }
    }
}
