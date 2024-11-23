using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.SceneManagement;
public class UIInventory : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Image spriteHeadNpc;
    public NpcManager npcManager;
    public NpcClass npcSelecying;
    public List<InvenrotySlots> listInvenrotySlotsUI = new List<InvenrotySlots>();
    public event Action<ItemWeapon> OnWeaponChanged;
    public List<ItemData> listItemDataInventoryEqicment;
    public List<ItemData> listItemDataInventoryslot;
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

        if (itemClass.itemtype == Itemtype.Ammo || itemClass.itemtype == Itemtype.General)
        {
            itemData = listItemDataInventoryslot.FirstOrDefault(itemnpc => itemnpc.idItem == itemClass.idItem);
        }
        else
        {
            itemData = listItemDataInventoryEqicment.FirstOrDefault(itemnpc => itemnpc.idItem == itemClass.idItem);

        }

        if (itemData != null)
        {
            itemData.count--;
            if (itemData.count == 0)
            {
                if (itemClass.itemtype == Itemtype.Ammo || itemClass.itemtype == Itemtype.General)
                {
                    listItemDataInventoryslot.Remove(itemData);
                }

                else
                {
                    listItemDataInventoryEqicment.Remove(itemData);
                }

            }
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

        // Iterate through inventory slots
        for (int i = listItemDataInventoryslot.Count - 1; i >= 0; i--) // Reverse loop for safe removal
        {
            ItemData itemData = listItemDataInventoryslot.ElementAt(i);

            if (itemData.count == 0)
            {
                listItemDataInventoryslot.RemoveAt(i); // Remove item with count 0
            }
            else
            {
                InvenrotySlots inventortSlot = listInvenrotySlotsUI.ElementAt(i);
                GameObject uIItem = CreateUIItem(itemData, inventortSlot);
            }
        }

        // Iterate through equipment slots
        for (int i = listItemDataInventoryEqicment.Count - 1; i >= 0; i--) // Reverse loop for safe removal
        {
            ItemData itemData = listItemDataInventoryEqicment.ElementAt(i);

            if (itemData.count == 0)
            {
                listItemDataInventoryEqicment.RemoveAt(i); // Remove item with count 0
            }
            else
            {
                InvenrotySlots inventortEqicment = listInvenrotySlotsUI.ElementAt(i + 12);
                SlotType slotTypeSlot = inventortEqicment.slotTypeInventory;
                Itemtype itemDatatype = itemData.itemtype;

                // Check if the item matches the slot type
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
    }

    public StatAmplifier statAmplifier;
    public void SelectNpcDefenseScene()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        statAmplifier = FindObjectOfType<StatAmplifier>();
        statAmplifier.endurance = npcSelecying.endurance;
        statAmplifier.combat = npcSelecying.combat;
        statAmplifier.speed = npcSelecying.speed;
        Debug.Log("Npc endurance : " + statAmplifier.endurance);
        Debug.Log("Npc endurance  Select : " + npcSelecying.endurance);
        // Assign the NPC's specialist role to the StatAmplifier
        statAmplifier.specialistRole = npcSelecying.roleNpc;
        statAmplifier.InitializeAmplifiers(); // Recalculate multipliers
        statAmplifier.ApplyRoleModifiers();   // Apply role modifiers

        // Update player and weapon stats if necessary
        player.currentStamina = player.GetMaxStamina();
        SetCostumeNpcExpentdition(npcSelecying, player.gameObject);
    }

    public void ClearItemDataInAllInventorySlotToListDataBoxes()
    {

      
        Debug.Log("ClearItemDataInAllInventorySlotToListDataBoxes");
        foreach (InvenrotySlots slotsItem in listInvenrotySlotsUI)
        {
            ItemClass itemClass = slotsItem.GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                // Debug.Log("item count : "+itemClass.quantityItem);
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
        ItemData weaponItemData = listItemDataInventoryEqicment.FirstOrDefault(item => item.itemtype == Itemtype.Weapon);

        if (weaponItemData != null)
        {
            // Get the corresponding UIItemData using idItem
            UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab
                .FirstOrDefault(uiItem => uiItem.idItem == weaponItemData.idItem);

            if (uiItemData != null)
            {
                // Get the ItemWeapon component
                ItemWeapon itemWeapon = uiItemData.GetComponent<ItemWeapon>();

                if (itemWeapon != null)
                {
                    // Invoke the event with the new weapon
                    OnWeaponChanged?.Invoke(itemWeapon);
                }
                else
                {
                    Debug.LogWarning("ItemWeapon component not found on UIItemData.");
                    OnWeaponChanged?.Invoke(null); // No weapon
                }
            }
            else
            {
                Debug.LogWarning("UIItemData not found for idItem: " + weaponItemData.idItem);
                OnWeaponChanged?.Invoke(null); // No weapon
            }
        }
        else
        {
            // No weapon equipped
            OnWeaponChanged?.Invoke(null);
        }
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
    private void InstallNpcCostumeOnPlayer(GameObject playerObject, NpcClass npcClass)
    {
        // Get the NpcCoutume component from the player
        NpcCoutume playerCoutume = playerObject.GetComponent<NpcCoutume>();
        if (playerCoutume != null)
        {
            // Get the NpcManager instance
            if (npcManager == null)
            {
                npcManager = FindObjectOfType<NpcManager>();
            }

            // Retrieve the costume data based on the selected NPC
            HeadCoutume headCoutume = npcManager.listHeadCoutume.FirstOrDefault(coutume => coutume.idHead == npcClass.idHead);
            BodyCoutume bodyCoutume = npcManager.listBodyCoutume.FirstOrDefault(coutume => coutume.idBody == npcClass.idBody);
            FeedCoutume feedCoutume = npcManager.listFeedCoutume.FirstOrDefault(coutume => coutume.idFeed == npcClass.idFeed);

            // Apply the costume to the player's NpcCoutume
            playerCoutume.SetCostume(headCoutume, bodyCoutume, feedCoutume);
        }
        else
        {
            Debug.LogWarning("Player does not have a NpcCoutume component.");
        }
    }

}

