using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
public class UIInventory : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Image spriteHeadNpc;
    public NpcManager npcManager;
    public NpcClass npcSelecying;
    public List<InvenrotySlots> listInvenrotySlotsUI = new List<InvenrotySlots>();
    public List<ItemData> listItemDataInventoryslot;
    public List<ItemData> listItemDataInventoryEqicment;
    public Transform transformBoxes;
    public InventoryItemPresent inventoryItemPresent;
    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI levelEnduranceText;
    public TextMeshProUGUI levelCombatText;
    public TextMeshProUGUI levelSpeedText;
    public TextMeshProUGUI specialistNpcText;
    public void RemoveItemData(ItemClass itemClass)
    {   
        ItemData itemData = new ItemData();  
        if(itemClass.itemtype == Itemtype.Ammo || itemClass.itemtype == Itemtype.General)
            itemData= listItemDataInventoryslot.FirstOrDefault(itemnpc => itemnpc.idItem == itemClass.idItem);
        else 
            itemData= listItemDataInventoryEqicment.FirstOrDefault(itemnpc => itemnpc.idItem == itemClass.idItem);
        
        if (itemData != null)
        {
            itemData.count--;
        }
        else 
        {
            return;
        }
    }
    
    public void AddITemlistInvenrotySlots(ItemClass itemClass)
    {
        ItemData itemData = listItemDataInventoryslot.FirstOrDefault(item => item.idItem == itemClass.idItem);
        if (listItemDataInventoryslot.Count < npcSelecying.countInventorySlot)
        {
            if (itemData != null)
            {
                if (itemData.count + itemClass.quantityItem <= itemData.maxCount)
                {
                    itemData.count += itemClass.quantityItem;
                }
                else
                {
                    ItemData newitemData = itemData;
                    newitemData.count = itemClass.quantityItem - (itemData.maxCount - itemData.count);
                    listItemDataInventoryslot.Add(newitemData);
                    itemData.count = itemData.maxCount;
                }
            }
            else
            {
                listItemDataInventoryslot.Add(itemData);
            }
        }
        else
        {
            if (itemData != null)
            {
                itemData.count = itemClass.maxCountItem;
            }
            else
                return;
        }
    }
    public void SetCostumeNpcExpentdition(NpcClass npcClass, GameObject npcOBJ)
    {
        HeadCoutume headCoutume = npcManager.listHeadCoutume.FirstOrDefault(coutume => coutume.idHead == npcClass.idHead);
        BodyCoutume bodyCoutume = npcManager.listBodyCoutume.FirstOrDefault(coutume => coutume.idBody == npcClass.idBody);
        FeedCoutume feedCoutume = npcManager.listFeedCoutume.FirstOrDefault(coutume => coutume.idFeed == npcClass.idFeed);

        NpcCoutume npcCoutume = npcOBJ.GetComponent<NpcCoutume>();

        npcCoutume.SetCostume(headCoutume, bodyCoutume, feedCoutume);
    }
    public void SetValuableUIInventory()
    {

        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        inventoryItemPresent.targetObject = this.gameObject;

        npcManager = FindObjectOfType<NpcManager>();
        npcManager.dropdown = this.dropdown;
        npcManager.uIInventory = this;
        npcManager.levelCombatText = levelEnduranceText;
        npcManager.levelEnduranceText = levelCombatText;
        npcManager.levelSpeedText = levelSpeedText;
        npcManager.specialistNpcText = specialistNpcText;

        SetSlotToInventory();

        dropdown.onValueChanged.AddListener(npcManager.OnDropdownValueChanged);

        npcManager.SetOptionDropDown();
        npcManager.OnDropdownValueChanged(0);

        inventoryItemPresent.RefreshUIBox();
    }
    public void SetSlotToInventory()
    {
        inventoryItemPresent.listInvenrotySlots.Clear();

        inventoryItemPresent.invenrotySlotSpecialMilitaryLock = listInvenrotySlotsUI.ElementAt(13);
        inventoryItemPresent.invenrotySlotSpecialScavengerLock = listInvenrotySlotsUI.ElementAt(16);

        for (int i = 0; i < 12; i++)
        {
            inventoryItemPresent.listInvenrotySlots.Add(listInvenrotySlotsUI.ElementAt(i));
        }

        inventoryItemPresent.transformsBoxes = transformBoxes;
    }
    public void RefreshUIBoxCategory(int numCategory)
    {
        inventoryItemPresent.ClearUIBoxes();

        Itemtype itemtypeCategory = (Itemtype)numCategory;

        foreach (ItemData itemData in inventoryItemPresent.listItemsDataBox)
        {
            if (itemData.itemtype == itemtypeCategory)
            {
                inventoryItemPresent.CreateUIItemInBoxes(itemData);

            }
        }

    }
    public void RefreshUIInventory()
    {
        ClearAllChildInvenrotySlot();


        for (int i = 0; i < listItemDataInventoryslot.Count; i++)
        {
            InvenrotySlots inventortSlot = listInvenrotySlotsUI.ElementAt(i);
            ItemData itemData = listItemDataInventoryslot.ElementAt(i);
            GameObject uIItem = CreateUIItem(itemData, inventortSlot);

        }

        for (int i = 0; i < listItemDataInventoryEqicment.Count; i++)
        {
            InvenrotySlots inventortEqicment = listInvenrotySlotsUI.ElementAt(i + 12);
            ItemData itemData = listItemDataInventoryEqicment.ElementAt(i);
            SlotType slotTypeSlot = inventortEqicment.slotTypeInventory;
            Itemtype itemDatatype = itemData.itemtype;
            if (slotTypeSlot == SlotType.SlotWeapon && itemDatatype == Itemtype.Weapon ||
            slotTypeSlot == SlotType.SlotVest && itemDatatype == Itemtype.Vest ||
            slotTypeSlot == SlotType.SlotBackpack && itemDatatype == Itemtype.Backpack ||
            slotTypeSlot == SlotType.SlotTool && itemDatatype == Itemtype.Tool ||
            slotTypeSlot == SlotType.SlotGrenade && itemDatatype == Itemtype.Grenade)
            {
                GameObject uIItemEqicment = CreateUIItem(itemData, inventortEqicment);
            }

        }

    }
    public void SelectNpcDefenseScene()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        StatAmplifier statAmplifier = FindObjectOfType<StatAmplifier>();

        statAmplifier.endurance = npcSelecying.endurance;
        statAmplifier.combat = npcSelecying.combat;
        statAmplifier.speed = npcSelecying.speed;

        // Assign the NPC's specialist role to the StatAmplifier
        statAmplifier.specialistRole = npcSelecying.roleNpc;
        statAmplifier.ApplyRoleModifiers();

        // Update player and weapon stats if necessary
        player.currentStamina = player.GetMaxStamina();
    }
    private void OnDestroy()
    {
        ClearItemDataInAllInventorySlotToListDataBoxes();
    }
    public void ClearItemDataInAllInventorySlotToListDataBoxes()
    {
        foreach (InvenrotySlots slotsItem in listInvenrotySlotsUI)
        {
            ItemClass itemClass = slotsItem.GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                inventoryItemPresent.AddItem(itemData);

                Destroy(itemClass.gameObject);
            }
        }
    }
    public void ClearAllChildInvenrotySlot()
    {
        foreach (InvenrotySlots slotsItem in listInvenrotySlotsUI)
        {
            ItemClass itemClass = slotsItem.GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                Destroy(itemClass.gameObject);
            }
        }
    }
    public void ConventAllUIItemInListInventorySlotToListItemData(List<ItemData> listSlotItemDatas)
    {
        for (int i = 0; i < 12; i++)
        {
            ItemClass itemClass = listInvenrotySlotsUI.ElementAt(i).GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                //  Debug.Log("Item Class quantityItem : " + itemClass.quantityItem);
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);

                // Debug.Log(itemData.count);
                listSlotItemDatas.Add(itemData);
            }
        }
    }
    public void ConventAllUIItemInListInventorySlotToListEqicmentItemData(List<ItemData> listEqicmentItemDatas)
    {
        for (int i = 12; i < 19; i++)
        {
            ItemClass itemClass = listInvenrotySlotsUI.ElementAt(i).GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                listEqicmentItemDatas.Add(itemData);
            }
        }
    }
    public void ConventDataUIToItemData()
    {
        listItemDataInventoryslot.Clear();
        listItemDataInventoryEqicment.Clear();
        ConventAllUIItemInListInventorySlotToListItemData(listItemDataInventoryslot);
        ConventAllUIItemInListInventorySlotToListEqicmentItemData(listItemDataInventoryEqicment);
    }
    public GameObject CreateUIItem(ItemData itemData, InvenrotySlots invenrotySlots)

    {
        // List<UIItemData> listUIItemPrefab = ;
        GameObject itemUI = inventoryItemPresent.listUIItemPrefab.FirstOrDefault(idItem => idItem.idItem == itemData.idItem).gameObject;
        GameObject itemUICreate = Instantiate(itemUI, invenrotySlots.transform, true);

        UIItemData uIItemData = itemUICreate.GetComponent<UIItemData>();
        ItemClass itemClass = itemUICreate.GetComponent<ItemClass>();

        itemClass.quantityItem = itemData.count;
        itemClass.maxCountItem = itemData.maxCount;

        uIItemData.slotTypeParent = invenrotySlots.slotTypeInventory;
        uIItemData.UpdateDataUI(itemClass);

        return itemUI;
    }

}

