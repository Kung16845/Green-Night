using UnityEngine;

public class ArmourEquip : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Weapon weapon;
    private UIInventory uiInventory;
    private float DamageIncreasePercent;
    private float SpeedIncreasePercent;
    private float StaminaIncreasePercent;
    private int? currentVestId = null;
    private float storedweapondamge;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        weapon = GetComponentInChildren<Weapon>();

        uiInventory = FindObjectOfType<UIInventory>();
        if (uiInventory != null)
        {
            uiInventory.OnVestChanged += ApplyVestStats;
        }
    }

    void ApplyVestStats(ItemVest vest)
    {
        if (vest != null)
        {
            if (currentVestId == vest.idItem)
            {
                Debug.Log("Same Vest equipped. Skipping re-initialization.");
                return;
            }
            currentVestId = vest.idItem; // Update the current weapon ID
            if (playerMovement != null)
            {
                SpeedIncreasePercent = vest.speedIncreasePercent;
                StaminaIncreasePercent =  vest.staminaIncreasePercent;
                playerMovement.baseSpeed *= SpeedIncreasePercent;
                playerMovement.baseSprintSpeed *= SpeedIncreasePercent;
                playerMovement.baseMaxStamina *= StaminaIncreasePercent;
                playerMovement.currentStamina = Mathf.Min(playerMovement.currentStamina, playerMovement.baseMaxStamina);
            }

            // Apply stats to Weapon
            if (weapon != null)
            {
                if(weapon.damage != 0)
                {
                    storedweapondamge = weapon.damage;
                    Debug.Log(storedweapondamge);
                    DamageIncreasePercent = vest.damageIncreasePercent;
                    weapon.damage *= DamageIncreasePercent;
                }
            }

            Debug.Log($"Applied Vest Stats: Damage +{vest.damageIncreasePercent}%, Speed +{vest.speedIncreasePercent}%, Stamina +{vest.staminaIncreasePercent}%");
        }
        else
        {
            ResetStats();
            currentVestId  = null; 
            storedweapondamge = 0;
        }
    }

    void ResetStats()
    {
        if (playerMovement != null)
        {
            playerMovement.baseSpeed = 2f;
            playerMovement.baseSprintSpeed = 4f;
            playerMovement.baseMaxStamina = 100f;
        }
        DamageIncreasePercent = 1;
        SpeedIncreasePercent = 1;
        StaminaIncreasePercent = 1;
        if(storedweapondamge != 0)
        {
            Debug.Log(storedweapondamge);
            weapon.damage = storedweapondamge;
        }
        Debug.Log("Reset stats to defaults.");
    }
}
