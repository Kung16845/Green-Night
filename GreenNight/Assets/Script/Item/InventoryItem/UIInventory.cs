using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventory : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public NpcManager npcManager;
    public List<InvenrotySlots> listInvenrotySlots = new List<InvenrotySlots>();
    public InventoryItemPresent inventoryItemPresent;

    private void Awake()
    {
        npcManager = FindObjectOfType<NpcManager>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

        npcManager.dropdown = this.dropdown;
        dropdown.onValueChanged.AddListener(npcManager.OnDropdownValueChanged);
        npcManager.SetOptionDropDown();
        npcManager.OnDropdownValueChanged(0);
    }
    public void ClearItemDataInAllInventorySlot()
    {
        foreach (InvenrotySlots slotsItem in listInvenrotySlots)
        {
            ItemClass itemClass = slotsItem.GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                inventoryItemPresent.AddItem(itemData);
            }
        }
    }
    private void OnDestroy()
    {
        ClearItemDataInAllInventorySlot();
    }
}

