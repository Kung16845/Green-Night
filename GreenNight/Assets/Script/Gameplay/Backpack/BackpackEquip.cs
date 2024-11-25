using UnityEngine;

public class BackpackEquip : MonoBehaviour
{
    private UIInventory uiInventory;
    private int? currentBackpackId = null;
    private int defaultInventorySlotCount;
    private int slotIncreaseAmount;

    void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>();
        if (uiInventory != null)
        {
            uiInventory.OnBackpackChanged += ApplyBackpackStats;
            defaultInventorySlotCount = uiInventory.npcSelecying.countInventorySlot;
        }
        else
        {
            Debug.LogError("NpcClass component not found on this GameObject.");
        }
    }

    void ApplyBackpackStats(ItemBackpack backpack)
    {
        if (backpack != null)
        {
            if (currentBackpackId == backpack.idItem)
            {
                Debug.Log("Same Backpack equipped. Skipping re-initialization.");
                return;
            }
            currentBackpackId = backpack.idItem;
            slotIncreaseAmount = backpack.slotIncreasing;
            uiInventory.npcSelecying.countInventorySlot = defaultInventorySlotCount + slotIncreaseAmount;
        }
        else
        {
            ResetStats();
            currentBackpackId = null;
            slotIncreaseAmount = 0;
        }
    }

    void ResetStats()
    {
        if (uiInventory != null)
        {
            uiInventory.npcSelecying.countInventorySlot = defaultInventorySlotCount;
            Debug.Log("Reset inventory slots to default.");
        }
    }

    private void OnDisable()
    {
        if (uiInventory != null)
        {
            uiInventory.OnBackpackChanged -= ApplyBackpackStats;
        }
    }
}
