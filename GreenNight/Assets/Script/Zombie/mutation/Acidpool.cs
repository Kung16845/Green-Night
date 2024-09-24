using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public static AcidPool Instance { get; private set; }

    [Header("Damage Settings")]
    private float barrierDamagePerTick;   // Damage dealt to barriers each tick
    private float zombieDamagePerTick;    // Damage dealt to zombies each tick
    private float duration;               // Total duration of the acid pool
    private float radius;                 // Effective radius of the acid pool
    private float damageInterval;         // Time between each damage tick

    [Header("Visual Settings")]
    public SpriteRenderer spriteRenderer; // Optional: Assign a SpriteRenderer for visual representation

    public void Initialize(float barrierDamage, float zombieDamage, float duration, float radius, float interval)
    {
        // If an acid pool already exists, reposition and reinitialize it
        if (Instance != null && Instance != this)
        {
            Instance.Reinitialize(barrierDamage, zombieDamage, duration, radius, interval, transform.position);
            Destroy(this.gameObject);
            return;
        }

        // Initialization code as before...
        this.barrierDamagePerTick = barrierDamage;
        this.zombieDamagePerTick = zombieDamage;
        this.duration = duration;
        this.radius = radius;
        this.damageInterval = interval;

        if (spriteRenderer != null)
        {
            spriteRenderer.transform.localScale = Vector3.one * radius * 2;
        }

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
            collider.radius = radius;
        }
        else
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = radius;
        }

        StartCoroutine(ApplyDamageOverTime());
    }
    public void Reinitialize(float barrierDamage, float zombieDamage, float duration, float radius, float interval, Vector3 newPosition)
    {
        this.barrierDamagePerTick = barrierDamage;
        this.zombieDamagePerTick = zombieDamage;
        this.duration = duration;
        this.radius = radius;
        this.damageInterval = interval;

        transform.position = newPosition;

        if (spriteRenderer != null)
        {
            spriteRenderer.transform.localScale = Vector3.one * radius * 2;
        }

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = radius;
        }

        StopAllCoroutines();
        StartCoroutine(ApplyDamageOverTime());
    }
    private IEnumerator ApplyDamageOverTime()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Find all colliders within the radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D collider in colliders)
            {
                // Apply damage to Zombies
                Zombie zombie = collider.GetComponent<Zombie>();
                if (zombie != null)
                {
                    zombie.ZombieTakeDamage(zombieDamagePerTick, DamageType.Acid);
                }

                // Apply damage to Barriers
                Barrier barrier = collider.GetComponent<Barrier>();
                if (barrier != null)
                {
                    barrier.BarrierTakeDamage(barrierDamagePerTick);
                }
            }

            // Wait for the next damage tick
            yield return new WaitForSeconds(damageInterval);
            elapsed += damageInterval;
        }

        // Destroy the acid pool after duration
        Destroy(this.gameObject);
    }
}
