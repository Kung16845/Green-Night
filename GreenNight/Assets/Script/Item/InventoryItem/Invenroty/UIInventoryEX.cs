using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryEX : UIInventory
{
    public List<ItemData> listItemDataInventoryslot;
    public List<ItemData> listItemDataInventoryEqicment;
    private void Awake()
    {
        SetValuableUIInventory();
    }

    public void RefreshUIInventory()
    {   
        listItemDataInventoryslot.Clear();
        ConventAllUIItemInListInventorySlotToListItemData(listItemDataInventoryslot);
        ClearAllChildInvenrotySlot();
        Debug.Log(listItemDataInventoryslot.Count);
        for (int i = 0; i < listItemDataInventoryslot.Count; i++)
        {   
            
            InvenrotySlots inventortSlot = listInvenrotySlotsUI.ElementAt(i);
            ItemData itemData = listItemDataInventoryslot.ElementAt(i);
            GameObject uIItem = CreateUIItem(itemData, inventortSlot);

        }

    }


}
