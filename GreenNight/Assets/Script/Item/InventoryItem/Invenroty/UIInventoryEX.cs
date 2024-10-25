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
    private void Start() {
         RefreshUIInventory();
    }
    public void RefreshUIInventory()
    {   
        ClearAllChildInvenrotySlot();

        Debug.Log(listItemDataInventoryslot.Count);
        for (int i = 0; i < listItemDataInventoryslot.Count; i++)
        {   
            InvenrotySlots inventortSlot = listInvenrotySlotsUI.ElementAt(i);
            ItemData itemData = listItemDataInventoryslot.ElementAt(i);
            GameObject uIItem = CreateUIItem(itemData, inventortSlot);

        }
        for(int i = 0 ; i < listItemDataInventoryEqicment.Count ; i++)
        {
            InvenrotySlots inventortEqicment = listInvenrotySlotsUI.ElementAt(i+12);
            ItemData itemData = listItemDataInventoryEqicment.ElementAt(i);
            SlotType slotTypeSlot = inventortEqicment.slotTypeInventory;
            Itemtype itemDatatype = itemData.itemtype;
            if(slotTypeSlot == SlotType.SlotWeapon && itemDatatype == Itemtype.Weapon || 
            slotTypeSlot == SlotType.SlotVest && itemDatatype == Itemtype.Vest || 
            slotTypeSlot == SlotType.SlotBackpack && itemDatatype == Itemtype.Backpack ||
            slotTypeSlot == SlotType.SlotTool && itemDatatype == Itemtype.Tool ||
            slotTypeSlot == SlotType.SlotGrenade && itemDatatype == Itemtype.Grenade)
            {
                GameObject uIItemEqicment = CreateUIItem( itemData,inventortEqicment);
            }
            
        }

    }
    private void OnEnable() 
    {
       RefreshUIInventory();
    }
    private void OnDisable()
    {
        ConventDataUIToItemData();
    }
    public void ConventDataUIToItemData()
    {
        listItemDataInventoryslot.Clear();
        listItemDataInventoryEqicment.Clear();
        ConventAllUIItemInListInventorySlotToListItemData(listItemDataInventoryslot);
        ConventAllUIItemInListInventorySlotToListEqicmentItemData(listItemDataInventoryEqicment);
    }

}
