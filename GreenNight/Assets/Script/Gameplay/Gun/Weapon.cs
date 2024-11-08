using System.Collections;
using UnityEngine;
using System.Linq;
public class Weapon : MonoBehaviour
{
    
    // Weapon properties
    public int rateOfFire; // Rounds per minute
    public float handling; // Speed penalty and reload speed (1-100)
    public float accuracy; // Bullet precision (1-100)
    public int capacity; // Ammo in magazine (1-100)
    public float stability; // Recoil (1-100)
    public float damage; // Damage per bullet
    public bool fullAuto; // Full auto or not
    public bool isShotgun; // Determines if the weapon is a shotgun
    public int pellets; // Number of pellets for shotgun
    public float spreadAngle; // Spread angle for shotgun
    public int stabilityThreshold = 5; // Number of shots before stability penalty starts
    public CaliberType caliberType;
    private AnimationController animationController;
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
    private ActionController actionController;
    private UIInventory uiInventory;
    private InventoryItemPresent inventoryItemPresent;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        statAmplifier = GetComponent<StatAmplifier>();
        animationController = GetComponent<AnimationController>();
        actionController = GetComponent<ActionController>();

        uiInventory = FindObjectOfType<UIInventory>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        // DisableWeapon();
        if (uiInventory != null)
        {
            // Subscribe to the OnWeaponChanged event
            uiInventory.OnWeaponChanged += UpdateWeaponStats;
        }
        else
        {
            Debug.LogWarning("UIInventory not found.");
        }

    }
    private void UpdateWeaponStats(ItemWeapon itemWeapon)
    {
        if (itemWeapon != null)
        {
            // Apply stats from ItemWeapon to Weapon class
            rateOfFire = itemWeapon.rateOfFire;
            handling = itemWeapon.handling;
            accuracy = itemWeapon.accuracy;
            capacity = itemWeapon.capacity;
            stability = itemWeapon.stability;
            damage = itemWeapon.damage;
            fullAuto = itemWeapon.fullAuto;
            isShotgun = itemWeapon.isShotgun;
            pellets = itemWeapon.Pellets;
            spreadAngle = itemWeapon.Spreadangle;
            caliberType = ConvertAmmoTypeToCaliberType(itemWeapon.ammoType);

            // Update derived values
            currentAmmo = capacity;
            fireRate = 60f / rateOfFire;
            initialAccuracy = accuracy;
            animationController.isgunequip = true;
            // Recalculate stat amplifiers
            if (statAmplifier != null)
            {
                statAmplifier.InitializeAmplifiers(); // Recalculate multipliers
                statAmplifier.ApplyRoleModifiers();   // Apply role modifiers
            }

            // Apply handling penalty and stat amplifiers
            ApplyHandlingPenalty();
            ApplyStatAmplifier();

            Debug.Log("Weapon stats updated.");
        }
        else
        {
            // No weapon equipped, reset stats or disable weapon functionality
            Debug.Log("No weapon equipped. Weapon functionality disabled.");
            DisableWeapon();
        }
    }

    private void DisableWeapon()
    {
        // Reset stats
        rateOfFire = 0;
        handling = 0;
        accuracy = 0;
        capacity = 0;
        stability = 0;
        damage = 0;
        fullAuto = false;
        isShotgun = false;
        pellets = 0;
        spreadAngle = 0;
        caliberType = CaliberType.Low; // Add a 'None' type if needed

        // Additional logic to disable shooting
        currentAmmo = 0;
        isReloading = false;
        shotsFiredConsecutively = 0;
        animationController.isgunequip = false;
    }
    private void InitializeWeaponStats()
    {
        // Use the existing OnWeaponChanged logic
        ItemData weaponItemData = uiInventory.listItemDataInventoryEqicment
            .FirstOrDefault(item => item.itemtype == Itemtype.Weapon);

        if (weaponItemData != null)
        {
            UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab
                .FirstOrDefault(uiItem => uiItem.idItem == weaponItemData.idItem);

            if (uiItemData != null)
            {
                ItemWeapon itemWeapon = uiItemData.GetComponent<ItemWeapon>();
                if (itemWeapon != null)
                {
                    UpdateWeaponStats(itemWeapon);
                }
            }
        }
        else
        {
            DisableWeapon();
        }
    }
    private CaliberType ConvertAmmoTypeToCaliberType(Ammotype ammoType)
    {
        switch (ammoType)
        {
            case Ammotype.HighCaliber:
                return CaliberType.High;
            case Ammotype.MediumCaliber:
                return CaliberType.Medium;
            case Ammotype.LowCaliber:
                return CaliberType.Low;
            case Ammotype.Shotgun:
                return CaliberType.Shotgun;
            default:
                return CaliberType.Low;
        }
    }

    void Update()
    {
        if (actionController != null && actionController.canuseweapon)
        {
            if (isReloading) return;

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
                    RecoverAccuracy();
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
                    RecoverAccuracy();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Shoot()
    {
        if (currentAmmo <= 0)
        {
            return;
        }

        currentAmmo--;

        if (isShotgun)
        {
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // Set the caliber type and initial penetration count at the time of bullet instantiation
        bulletScript.caliberType = caliberType;
        bulletScript.InitializePenetration();  // Initialize penetration

        bulletScript.damage = damage;

        // Get the world position of the mouse
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Ensure the z-axis is the same since we are working in 2D
        mouseWorldPosition.z = 0;

        // Calculate the direction from the fire point to the mouse, and normalize it
        Vector2 bulletDirection = ((Vector2)mouseWorldPosition - (Vector2)firePoint.position).normalized;

        // Apply some accuracy adjustments
        float accuracyFactor = GetAccuracyFactor();
        float accuracySpread = (1 - accuracyFactor) / 10f;
        bulletDirection += new Vector2(Random.Range(-accuracySpread, accuracySpread), Random.Range(-accuracySpread, accuracySpread));

        // Set bullet velocity in the direction of the mouse
        rb.velocity = bulletDirection * 70f;
         DDAdataCollector.Instance.OnBulletFired(bulletScript.bulletID);
    }


   private void FirePellet()
    {
        // Get the world position of the mouse
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Ensure the z-axis is the same since we are working in 2D
        mouseWorldPosition.z = 0;

        // Calculate the direction from the fire point to the mouse, and normalize it
        Vector2 aimDirection = ((Vector2)mouseWorldPosition - (Vector2)firePoint.position).normalized;

        // Apply random spread angle to the aim direction
        float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        Vector2 directionWithSpread = Quaternion.Euler(0, 0, angle) * aimDirection;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        bulletScript.caliberType = caliberType;

        // Apply some accuracy adjustments
        float accuracyFactor = GetAccuracyFactor();
        float accuracySpread = (1 - accuracyFactor) / 10f;
        directionWithSpread += new Vector2(Random.Range(-accuracySpread, accuracySpread), Random.Range(-accuracySpread, accuracySpread));

        // Set bullet velocity in the calculated direction
        rb.velocity = directionWithSpread * 70f;
         DDAdataCollector.Instance.OnBulletFired(bulletScript.bulletID);
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
            shotsFiredConsecutively = 0; 
        }
    }

    private void ApplyHandlingPenalty()
    {
        if (playerMovement != null && statAmplifier != null)
        {
            float handlingMultiplier = statAmplifier.GetHandlingMultiplier();

            playerMovement.baseSpeed = playerMovement.baseSpeed * (1f - (0.5f * (100f - handling) / 100f)) * handlingMultiplier;
            playerMovement.baseSprintSpeed = playerMovement.baseSprintSpeed * (1f - (0.3f * (100f - handling) / 100f)) * handlingMultiplier;
        }
    }


    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log((6.5f * (1-(statAmplifier.GetCombatMultiplier()-1)))  * (100f - handling) / 100f);
        float reloadTime = ((6.5f * (1-(statAmplifier.GetCombatMultiplier()-1)))  * (100f - handling) / 100f) * statAmplifier.GetReloadSpeedMultiplier();;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = capacity;
        isReloading = false;
        shotsFiredConsecutively = 0; 
        Debug.Log("Reloaded!");
    }
    private void ApplyStatAmplifier()
    {
        if (statAmplifier != null)
        {
            float combatMultiplier = statAmplifier.GetCombatMultiplier();
            float accuracyMultiplier = statAmplifier.GetAccuracyMultiplier();
            float handlingMultiplier = statAmplifier.GetHandlingMultiplier();
            float stabilityMultiplier = statAmplifier.GetStabilityMultiplier();
            float damageMultiplier = statAmplifier.GetDamageMultiplier();

            accuracy *= accuracyMultiplier;
            handling *= handlingMultiplier;
            stability *= stabilityMultiplier;
            damage *= damageMultiplier;

            // Clamp values if necessary
            accuracy = Mathf.Clamp(accuracy, 0, 100);
            handling = Mathf.Clamp(handling, 0, 100);
            stability = Mathf.Clamp(stability, 0, 100);
        }
    }
}
