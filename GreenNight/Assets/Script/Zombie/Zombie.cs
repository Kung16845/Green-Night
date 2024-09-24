using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{
    Bullet,
    Pulse,
    Fire,
    Acid,
    Explosive,
    // Future damage types can be added here
}
public enum MutationType
{
    None,
    Spike,
    Acid,
    Exploder,
    ArmourShell
}

public class Zombie : MonoBehaviour
{
    public Transform transformbarrier;
    public float currentHp;
    public float maxHp;
    public float maxArmourHp; 
    public float ArmourHp;
    public float currentSpeed;
    public float maxSpeed;
    public float attackDamage;
    public float attackTimer;
    public float countTImer;
    public Vector2 direction;
    public Rigidbody2D rb2D;
    public Barrier barrier;
    
    [Header("Damage Effects")]
    public float slowdownAmount = 0.25f;           // Amount to slow down (e.g., 0.5 means half speed)
    public float damageEffectDuration = 0.01f;       // Duration of the slowdown and red color effect

    private float originalSpeed;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageEffectCoroutine;
    private bool armourDepleted = false;

     [Header("Mutation Settings")]
    public MutationType mutationType = MutationType.None;
    [Range(1, 3)]
    public int mutationTier = 1;                  // Tier 1 to 3

    [Header("Acid Mutation Settings")]
    public GameObject acidPoolPrefab;
    public float[] acidBarrierDamage = { 5f, 7f, 9f };   // Damage per tick for barrier
    public float[] acidZombieDamage = { 7f, 10f, 15f };  // Damage per tick for zombies
    public float[] acidDuration = { 5f, 10f, 15f };      // Duration of acid pool
    public float[] acidRadius = { 2f, 3f, 4f };          // Effect radius per tier

    [Header("Exploder Mutation Settings")]
    public GameObject explosionPrefab;
    public float[] explosionBarrierDamage = { 50f, 70f, 100f };
    public float[] explosionZombieDamage = { 55f, 100f, 150f };
    public float[] explosionRadius = { 2f, 3f, 4f };

    private void Awake()
    {
        FindClosestBarrier();
        currentSpeed = maxSpeed;
        currentHp = maxHp;
        ArmourHp = maxArmourHp; // Initialize armor
        rb2D = GetComponent<Rigidbody2D>();
        CheckDirectionMove();
        originalSpeed = maxSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        ApplyMutationEffects();
    }

    public void ZombieAttack()
    {
        if (barrier != null)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                barrier.BarrierTakeDamage(attackDamage);
                countTImer = attackTimer;
            }
        }
    }

    public void CheckDirectionMove()
    {
        Vector2 toBarrier = transformbarrier.position - transform.position;

        if (Mathf.Abs(toBarrier.x) > Mathf.Abs(toBarrier.y))
        {
            // Move horizontally
            if (toBarrier.x > 0)
            {
                direction = Vector2.right;  // Move right
            }
            else
            {
                direction = Vector2.left;   // Move left
            }
        }
        else
        {
            // Move vertically
            if (toBarrier.y > 0)
            {
                direction = Vector2.up;     // Move up
            }
            else
            {
                direction = Vector2.down;   // Move down
            }
        }
    }

    public void FindClosestBarrier()
    {
        Barrier[] barriers = FindObjectsOfType<Barrier>();
        float closestDistance = Mathf.Infinity;
        Transform closestBarrierTransform = null;

        foreach (Barrier b in barriers)
        {
            float distanceToBarrier = Vector2.Distance(transform.position, b.transform.position);
            if (distanceToBarrier < closestDistance)
            {
                closestDistance = distanceToBarrier;
                closestBarrierTransform = b.transform;
            }
        }

        if (closestBarrierTransform != null)
        {
            transformbarrier = closestBarrierTransform;
            barrier = transformbarrier.GetComponent<Barrier>();
        }
    }

    public void ZombieMoveFindBarrier()
    {
        rb2D.velocity = direction * currentSpeed;
    }

     public void ZombieTakeDamage(float damage, DamageType damageType)
    {
        // Armor and health calculations
        if (ArmourHp > 0)
        {
            if (damageType == DamageType.Bullet)
            {
                // Reduce bullet damage by 5
                float reducedDamage = damage - 0.70f;
                if (reducedDamage < 0)
                    reducedDamage = 0f;

                ArmourHp -= reducedDamage;

                if (ArmourHp <= 0 && !armourDepleted)
                {
                    armourDepleted = true;
                    if (mutationType == MutationType.ArmourShell)
                    {
                        StartCoroutine(ArmourDepletedEffect());
                    }
                }

                // If armor is depleted, apply leftover damage to health
                if (ArmourHp < 0)
                {
                    currentHp += ArmourHp; // ArmourHp is negative
                    ArmourHp = 0f;
                }
            }
            else if (damageType == DamageType.Pulse)
            {
                // Remove all armor
                ArmourHp = 0f;

                if (!armourDepleted)
                {
                    armourDepleted = true;
                    if (mutationType == MutationType.ArmourShell)
                    {
                        StartCoroutine(ArmourDepletedEffect());
                    }
                }
            }
            else
            {
                // For other damage types, apply full damage to armor
                ArmourHp -= damage;

                if (ArmourHp <= 0 && !armourDepleted)
                {
                    armourDepleted = true;
                    if (mutationType == MutationType.ArmourShell)
                    {
                        StartCoroutine(ArmourDepletedEffect());
                    }
                }

                if (ArmourHp < 0)
                {
                    currentHp += ArmourHp; // ArmourHp is negative
                    ArmourHp = 0f;
                }
            }
        }
        else
        {
            // No armor left, apply damage to health
            currentHp -= damage;
        }

        // Apply damage effects: slowdown and sprite color change
        if (damageEffectCoroutine != null)
        {
            StopCoroutine(damageEffectCoroutine);
        }
        damageEffectCoroutine = StartCoroutine(DamageEffect());

        // Check for death
        if (currentHp <= 0)
        {
            OnDeath();
            Destroy(this.gameObject);
        }
    }
    private IEnumerator DamageEffect()
    {
        // Slow down the zombie
        currentSpeed = maxSpeed * slowdownAmount;

        // Change sprite color to red
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Wait for the effect duration
        yield return new WaitForSeconds(damageEffectDuration);

        // Reset speed and sprite color
        currentSpeed = originalSpeed;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
     private void OnDeath()
    {
        switch (mutationType)
        {
            case MutationType.Acid:
                ApplyAcidDeathEffect();
                break;
            case MutationType.Exploder:
                ApplyExploderDeathEffect();
                break;
            // Other mutations have no death effect
        }
    }
    private void ApplyMutationEffects()
    {
        switch (mutationType)
        {
            case MutationType.Spike:
                ApplySpikeMutation();
                break;
            case MutationType.ArmourShell:
                ApplyArmourShellMutation();
                break;
            // Acid and Exploder mutations have effects on death
        }
    }
     private void ApplySpikeMutation()
    {
        float damageMultiplier = 1f;
        switch (mutationTier)
        {
            case 1:
                damageMultiplier = 1.3f;
                break;
            case 2:
                damageMultiplier = 1.45f;
                break;
            case 3:
                damageMultiplier = 1.6f;
                break;
        }
        attackDamage *= damageMultiplier;
    }
     private void ApplyArmourShellMutation()
    {
        float extraArmourHp = 0f;
        switch (mutationTier)
        {
            case 1:
                extraArmourHp = 50f;
                break;
            case 2:
                extraArmourHp = 100f;
                break;
            case 3:
                extraArmourHp = 150f;
                break;
        }
        ArmourHp += extraArmourHp;
        maxArmourHp += extraArmourHp;
    }

    private IEnumerator ArmourDepletedEffect()
    {
        // Stop the zombie for 1 second
        float originalSpeed = currentSpeed;
        currentSpeed = 0f;

        yield return new WaitForSeconds(1f);

        // Resume speed
        currentSpeed = originalSpeed;
    }
    private void ApplyAcidDeathEffect()
{
    if (AcidPool.Instance != null)
    {
        // Reinitialize the existing acid pool instead of spawning a new one
        AcidPool.Instance.Reinitialize(
            acidBarrierDamage[mutationTier - 1],
            acidZombieDamage[mutationTier - 1],
            acidDuration[mutationTier - 1],
            acidRadius[mutationTier - 1],
            0.3f,
            transform.position
        );
    }
    else if (acidPoolPrefab != null)
    {
        GameObject acidPoolInstance = Instantiate(acidPoolPrefab, transform.position, Quaternion.identity);
        AcidPool acidPoolScript = acidPoolInstance.GetComponent<AcidPool>();
        if (acidPoolScript != null)
        {
            int index = mutationTier - 1; // Adjust for array indexing
            acidPoolScript.Initialize(
                acidBarrierDamage[index],
                acidZombieDamage[index],
                acidDuration[index],
                acidRadius[index],
                0.3f // Damage interval
            );
        }
    }
}


    private void ApplyExploderDeathEffect()
    {
        // Instantiate an explosion at the zombie's position
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            Explosion explosionScript = explosion.GetComponent<Explosion>();
            if (explosionScript != null)
            {
                int index = mutationTier - 1; // Adjust for array indexing
                explosionScript.Initialize(
                    explosionBarrierDamage[index],
                    explosionZombieDamage[index],
                    explosionRadius[index]
                );
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Barrier triggerbarrier = other.GetComponent<Barrier>();
        if (triggerbarrier != null)
        {
            barrier = triggerbarrier;
        }
    }
}
