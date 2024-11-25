using UnityEngine;

public class BackpackEquip : MonoBehaviour
{
    private UIInventory uiInventory;
    private ItemBackpack currentBackpack;
    private StatManager statManager;
    private int defaultInventorySlotCount;

    void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>();
        statManager = GetComponent<StatManager>();

        if (uiInventory != null)
        {
            uiInventory.OnBackpackChanged += OnBackpackChanged;
            defaultInventorySlotCount = uiInventory.npcSelecying.countInventorySlot;
        }
    }

    void OnBackpackChanged(ItemBackpack backpack)
    {
        currentBackpack = backpack;
        // Update inventory slots
        int slotIncreaseAmount = currentBackpack != null ? currentBackpack.slotIncreasing : 0;
        uiInventory.npcSelecying.countInventorySlot = defaultInventorySlotCount + slotIncreaseAmount;

        statManager.OnBackpackStatsChanged();
    }

    // Methods to provide modifiers to StatManager
    public float GetSpeedModifier()
    {
        return currentBackpack != null ? currentBackpack.IncreaseSpeed : 1f;
    }

    public float GetStaminaRecoverSpeedModifier()
    {
        return currentBackpack != null ? currentBackpack.StaminaRecoverSpeed : 1f;
    }

    void OnDisable()
    {
        if (uiInventory != null)
        {
            uiInventory.OnBackpackChanged -= OnBackpackChanged;
        }
    }
}
