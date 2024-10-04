using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Weapon properties
    public int rateOfFire; // Rounds per minute
    public int handling; // Speed penalty and reload speed (1-100)
    public float accuracy; // Bullet precision (1-100)
    public int capacity; // Ammo in magazine (1-100)
    public float stability; // Recoil (1-100)
    public float damage; // Damage per bullet
    public bool fullAuto; // Full auto or not
    public bool isShotgun; // Determines if the weapon is a shotgun
    public int pellets; // Number of pellets for shotgun
    public float spreadAngle; // Spread angle for shotgun
    public int stabilityThreshold = 5; // Number of shots before stability penalty starts

    // Internal variables
    [SerializeField] public int currentAmmo;
    [SerializeField] public float fireRate;
    [SerializeField] public float nextFireTime;
    [SerializeField] public bool isReloading = false;
    [SerializeField] private bool isNpc;
    private float initialAccuracy;
    public float accuracyPenalty; // Additional penalty to accuracy based on stability
    private StatAmplifier statAmplifier;
    private PlayerMovement playerMovement;
    public int shotsFiredConsecutively = 0; // Tracks the number of consecutive shots fired
    private bool isFiring; 

    // References
    public Transform firePoint; // Point from where bullets are fired
    public GameObject bulletPrefab; // Bullet prefab
    public Vector2 bulletDirection;

    void Start()
    {
        currentAmmo = capacity;
        fireRate = 60f / rateOfFire;
        initialAccuracy = accuracy;
        playerMovement = GetComponentInParent<PlayerMovement>();
        statAmplifier = GetComponent<StatAmplifier>(); // Ensure this is attached

        ApplyHandlingPenalty();
        ApplyStatAmplifier();
    }

    void Update()
    {
        if (isReloading) return;

        // Check if the mouse button is being held down
        isFiring = Input.GetMouseButton(0);

        if (fullAuto)
        {
            if (isFiring && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
                shotsFiredConsecutively++;
                if (shotsFiredConsecutively >= stabilityThreshold)
                {
                    ApplyStabilityPenalty();
                }
            }
            else if (!isFiring)
            {
                RecoverAccuracy(); // Only recover accuracy if the mouse button is released
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;

                shotsFiredConsecutively++;
                if (shotsFiredConsecutively >= stabilityThreshold)
                {
                    ApplyStabilityPenalty();
                }
            }
            else if (!isFiring)
            {
                RecoverAccuracy(); // Only recover accuracy if the mouse button is released
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        currentAmmo--;

        if (isShotgun)
        {
            // Shotgun logic: Fire multiple pellets
            for (int i = 0; i < pellets; i++)
            {
                FirePellet();
            }
        }
        else
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        // Instantiate bullet and set its direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;

        // Adjust bullet direction based on accuracy
        if (!isNpc)
        {
            bulletDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;
        }

        float accuracyFactor = GetAccuracyFactor();
        float accuracySpread = (1 - accuracyFactor) / 10f; // Calculate spread based on accuracy factor
        bulletDirection += new Vector2(Random.Range(-accuracySpread, accuracySpread), Random.Range(-accuracySpread, accuracySpread));
        rb.velocity = bulletDirection * 100f; // Adjust bullet speed as necessary
    }

    private void FirePellet()
    {
        // Calculate the initial direction towards the player's aim point
        Vector2 aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;

        // Apply the spread angle to the aim direction
        float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        Vector2 directionWithSpread = Quaternion.Euler(0, 0, angle) * aimDirection;

        // Instantiate pellet and set its direction with spread
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage / pellets; // Damage per pellet

        // Calculate the spread caused by accuracy
        float accuracyFactor = GetAccuracyFactor();
        float accuracySpread = (1 - accuracyFactor) / 10f; // Calculate spread based on accuracy factor
        directionWithSpread += new Vector2(Random.Range(-accuracySpread, accuracySpread), Random.Range(-accuracySpread, accuracySpread));

        // Set the pellet velocity based on the calculated direction
        rb.velocity = directionWithSpread * 1000f; // Adjust bullet speed as necessary
    }


    private float GetAccuracyFactor()
    {
        // Determine accuracy factor based on the accuracy value
        float baseAccuracy;
        if (accuracy <= 30)
        {
            baseAccuracy = 0.60f; // 60% accuracy
        }
        else if (accuracy <= 60)
        {
            baseAccuracy = 0.60f + (0.65f - 0.60f) * ((accuracy - 30) / 30f); // Interpolate between 60% and 65%
        }
        else if (accuracy <= 80)
        {
            baseAccuracy = 0.65f + (0.75f - 0.65f) * ((accuracy - 60) / 20f); // Interpolate between 65% and 75%
        }
        else
        {
            baseAccuracy = 0.75f + (1.0f - 0.75f) * ((accuracy - 80) / 20f); // Interpolate between 75% and 100%
        }

        // Adjust the accuracy based on the penalty
        float penaltyFactor = Mathf.Clamp01(accuracyPenalty / 25f); // Normalize penalty between 0 and 1
        return baseAccuracy * (1 - penaltyFactor); // Reduce base accuracy by penalty factor
    }



     private void ApplyStabilityPenalty()
    {
        float penaltyFactor;

        if (stability <= 40)
        {
            penaltyFactor = 0.25f; // Max penalty
        }
        else if (stability <= 60)
        {
            penaltyFactor = 0.25f - (0.25f * ((stability - 40) / 20f)); 
        }
        else if (stability <= 90)
        {
            penaltyFactor = 0.125f - (0.125f * ((stability - 60) / 30f));
        }
        else
        {
            penaltyFactor = 0f; // No penalty at max stability
        }

        accuracyPenalty = Mathf.Min(25f, accuracyPenalty + (penaltyFactor * Time.deltaTime * 100f * statAmplifier.GetCombatMultiplier())); // Amplified by combat stat
    }

    private void RecoverAccuracy()
    {
        accuracyPenalty = Mathf.Max(0f, accuracyPenalty - (50f * Time.deltaTime)); // Recover accuracy when not shooting
        if (accuracyPenalty == 0f)
        {
            shotsFiredConsecutively = 0; // Reset shot counter when accuracy fully recovers
        }
    }

    private void ApplyHandlingPenalty()
    {
        if (playerMovement != null)
        {
            playerMovement.currentSpeed = playerMovement.currentSpeed  * (1f - (0.5f * (100f - handling) / 100f)); 
            playerMovement.sprintSpeed = playerMovement.sprintSpeed * (1f - (0.3f * (100f - handling) / 100f)); 
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(3f * (100f - handling) / 100f); // Adjust reload speed based on handling
        currentAmmo = capacity;
        isReloading = false;
        shotsFiredConsecutively = 0; // Reset shot counter on reload
        Debug.Log("Reloaded!");
    }
    private void ApplyStatAmplifier()
    {
        stability *= statAmplifier.GetCombatMultiplier();
        accuracy *= statAmplifier.GetCombatMultiplier();
        fireRate /= statAmplifier.GetCombatMultiplier(); // Decreases reload time by amplifying fire rate
    }
}
