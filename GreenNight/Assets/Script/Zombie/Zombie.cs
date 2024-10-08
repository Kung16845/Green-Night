using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{

    LowcaliberBullet,
    MediumcaliberBullet,
    ShotgunPellet,
    HighcalliberBullet,
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
public enum ZombieState
{
    Moving,
    Attacking,
    Stopped,
    Dead,
    // Add other states as needed
}

public class Zombie : MonoBehaviour
{
    protected Dictionary<DamageType, float> damageMultipliers;
    private float speedMultiplier = 1f;     // Movement speed multiplier
    private float attackSpeedMultiplier = 1f;
    public float currentHp;
    public float maxHp;
    public float maxArmourHp; 
    public float ArmourHp;
    public float currentSpeed;
    public float maxSpeed;
    public float attackDamage;
    public float attackTimer;
    public float countTimer;
    public Rigidbody2D rb2D;
    public Barrier barrier;

    public float movementSpeed = 1.0f;       // Movement speed

    // Fields for Engaging Area
    public Lane currentLane;
    public bool isInEngagingArea = false;    // Whether the zombie is in the Engaging Area
    
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
    public float[] acidRadius = { 1f, 2f, 2.5f };          // Effect radius per tier

    [Header("Exploder Mutation Settings")]
    public GameObject explosionPrefab;
    public float[] explosionBarrierDamage = { 50f, 70f, 100f };
    public float[] explosionZombieDamage = { 55f, 100f, 150f };
    public float[] explosionRadius = { 2f, 3f, 4f };

    [Header("State Tracking")]
    public ZombieState currentState;
    private float damageEffectDurationRemaining;
    private float previousDirectionX = 1f;
    private void Awake()
    {
        currentSpeed = maxSpeed;
        currentHp = maxHp;
        ArmourHp = maxArmourHp; // Initialize armor
        rb2D = GetComponent<Rigidbody2D>();
        originalSpeed = maxSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        InitializeDamageMultipliers();
        ApplyMutationEffects();
    }
    public ZombieState CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }
    void Update()
    {
    }
    public void ZombieMoveFindBarrier()
    {
        // Move towards the attack point
        if (currentLane != null && currentLane.attackPoint != null)
        {
            currentState = ZombieState.Moving;
            Vector2 direction = (currentLane.attackPoint.position - transform.position).normalized;
            rb2D.velocity = direction * currentSpeed;

            previousDirectionX = direction.x;
            // Flip the sprite based on movement direction along the x-axis
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-0.7f, 0.7f, 1); // Flip left
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(0.7f, 0.7f, 1); // Face right
            }
        }
    }
    public bool HasReachedAttackPoint()
    {
        if (currentLane != null && currentLane.attackPoint != null)
        {
            float distanceToAttackPoint = Vector2.Distance(transform.position, currentLane.attackPoint.position);
            float thresholdDistance = 0.1f;

            if (distanceToAttackPoint <= thresholdDistance)
            {
                // Zombie has reached the attack point, set to Attacking state
                currentState = ZombieState.Attacking;

                // Keep the sprite direction based on the previous movement
                if (previousDirectionX < 0)
                {
                    transform.localScale = new Vector3(-0.7f, 0.7f, 1); // Keep facing left
                }
                else if (previousDirectionX > 0)
                {
                    transform.localScale = new Vector3(0.7f, 0.7f, 1); // Keep facing right
                }

                return true;
            }
        }
        return false;
    }


    public void ZombieAttack()
    {
        if (barrier != null)
        {
            if (countTimer > 0)
                countTimer -= Time.deltaTime * attackSpeedMultiplier;
            else
            {
                barrier.BarrierTakeDamage(attackDamage);
                countTimer = attackTimer;
            }
        }
    }
    public virtual void ZombieTakeDamage(float damage, DamageType damageType, float extraMultiplier = 1f)
    {
        // Calculate adjusted damage based on multipliers
        float multiplier = 1f;
        if (damageMultipliers != null && damageMultipliers.ContainsKey(damageType))
        {
            multiplier = damageMultipliers[damageType];
        }
        float adjustedDamage = damage * multiplier * extraMultiplier;
        if (ArmourHp > 0)
        {
            if (damageType == DamageType.HighcalliberBullet)
            {
                // Bullet damage reduces damage by 15% to armor
                float reducedDamage = adjustedDamage * 0.85f;
                ArmourHp -= reducedDamage;
                // Apply overflow damage to health
                ApplyOverflowDamageToHealth();
            }
            else if (damageType == DamageType.MediumcaliberBullet)
            {
                // Bullet damage reduces damage by 25% to armor
                float reducedDamage = adjustedDamage * 0.75f;
                ArmourHp -= reducedDamage;
                ApplyOverflowDamageToHealth();
            }
            else if (damageType == DamageType.LowcaliberBullet || damageType == DamageType.ShotgunPellet)
            {
                float reducedDamage = adjustedDamage * 0.50f;
                ArmourHp -= reducedDamage;
                ApplyOverflowDamageToHealth();
            }
            else if (damageType == DamageType.Explosive)
            {
                // Explosive damage splits between armor and health
                float damageToArmor = adjustedDamage * 0.60f;
                float damageToHealth = adjustedDamage * 0.40f;
                ArmourHp -= damageToArmor;
                currentHp -= damageToHealth;

                // Apply overflow damage to health
                ApplyOverflowDamageToHealth();
            }
            else if (damageType == DamageType.Pulse)
            {
                // Pulse damage removes all armor
                ArmourHp = 0f;
            }
            else
            {
                // Other damage types apply full damage to armor
                ArmourHp -= adjustedDamage;
                // Apply overflow damage to health
                ApplyOverflowDamageToHealth();
            }
        }
        else
        {
            currentHp -= adjustedDamage;
        }

        // Apply damage effects and check for death
        ApplyDamageEffects();
        CheckForDeath();
    }

    private void ApplyOverflowDamageToHealth()
    {
        if (ArmourHp < 0)
        {
            currentHp += ArmourHp; // ArmourHp is negative
            ArmourHp = 0f;
        }
    }

    private void ApplyDamageEffects()
    {
        if (damageEffectCoroutine != null)
        {
            StopCoroutine(damageEffectCoroutine);
            speedMultiplier = 1.0f;
            UpdateCurrentSpeed();
        }
        damageEffectCoroutine = StartCoroutine(DamageEffect());
    }

    private void CheckForDeath()
    {
        if (currentHp <= 0)
        {
            OnDeath();
            Destroy(this.gameObject);
        }
    }
    private IEnumerator DamageEffect()
    {
        // Slow down the zombie
        speedMultiplier = slowdownAmount;
        UpdateCurrentSpeed();

        // Change sprite color to red
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Wait for the effect duration
        yield return new WaitForSeconds(damageEffectDuration);

        // Reset speed multiplier to original value
        speedMultiplier = 1.0f;
        UpdateCurrentSpeed();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        damageEffectCoroutine = null;
    }

    protected virtual void OnDeath()
    {
        currentState = ZombieState.Dead;
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
    private void ApplyAcidDeathEffect()
    {
        // Instantiate an acid pool at the zombie's position
        if (acidPoolPrefab != null)
        {
            GameObject acidPool = Instantiate(acidPoolPrefab, transform.position, Quaternion.identity);
            AcidPool acidPoolScript = acidPool.GetComponent<AcidPool>();
            if (acidPoolScript != null)
            {
                int index = mutationTier - 1; // Adjust for array indexing
                acidPoolScript.Initialize(
                    acidBarrierDamage[index],
                    acidZombieDamage[index],
                    acidDuration[index],
                    acidRadius[index],
                    1// Damage interval
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
    protected virtual void InitializeDamageMultipliers()
    {
        damageMultipliers = new Dictionary<DamageType, float>
        {
            { DamageType.HighcalliberBullet, 1f },
            { DamageType.LowcaliberBullet, 1f },
            { DamageType.MediumcaliberBullet, 1f },
            { DamageType.ShotgunPellet, 1f },
            { DamageType.Pulse, 1f },
            { DamageType.Fire, 1f },
            { DamageType.Acid, 1f },
            { DamageType.Explosive, 1f },
        };
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Barrier triggerbarrier = other.GetComponent<Barrier>();
        if (triggerbarrier != null)
        {
            barrier = triggerbarrier;
        }
    }
    public void IncreaseSpeed(float multiplier)
    {
        speedMultiplier *= multiplier;
        UpdateCurrentSpeed();
    }

    public void ResetSpeed()
    {
        speedMultiplier = 1f;
        UpdateCurrentSpeed();
    }
    private void UpdateCurrentSpeed()
    {
        currentSpeed = maxSpeed * speedMultiplier;
    }
     public void IncreaseAttackSpeed(float multiplier)
    {
        attackSpeedMultiplier *= multiplier;
    }

    public void ResetAttackSpeed()
    {
        attackSpeedMultiplier = 1f;
    }
    public virtual void SetLane(Lane lane)
    {
        currentLane = lane;

        // Get the Barrier component from the attack point
        if (currentLane.attackPoint != null)
        {
            barrier = currentLane.attackPoint.GetComponent<Barrier>();
        }
    }
    public void SetTier(int tier)
    {
        mutationTier = tier;
        ApplyMutationEffects();
    }
     public void StopZombie()
    {
        currentState = ZombieState.Stopped;
        rb2D.velocity = Vector2.zero;
        // Additional logic for stopping
    }
    public void ResumeZombie()
    {
        currentState = ZombieState.Moving;
        // Additional logic for resuming movement
    }
    private void OnEnable()
    {
        ZombieManager.Instance.RegisterZombie(this);
    }

    private void OnDisable()
    {
        ZombieManager.Instance.UnregisterZombie(this);
    }
}
