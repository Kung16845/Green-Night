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
    public event Action<List<ItemWeapon>> OnWeaponsChanged;
    public event Action<ItemVest> OnVestChanged;
    public event Action<ItemBackpack> OnBackpackChanged;
    public List<ItemData> listItemDataInventoryEqicment;
    public List<ItemData> listItemDataInventorySlot;
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
            itemData = listItemDataInventorySlot.FirstOrDefault(itemnpc => itemnpc.idItem == itemClass.idItem);
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
                    listItemDataInventorySlot.Remove(itemData);
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
        ItemData itemData = listItemDataInventorySlot.FirstOrDefault(item => item.idItem == itemClass.idItem);
        if (listItemDataInventorySlot.Count < npcSelecying.countInventorySlot)
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
                    listItemDataInventorySlot.Add(newitemData);
                    itemData.count = itemData.maxCount;
                }
            }
            else
            {
                listItemDataInventorySlot.Add(itemData);
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
        npcManager.levelCombatText = levelCombatText;
        npcManager.levelEnduranceText = levelEnduranceText;
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
        inventoryItemPresent.UnlockSlotInventory(npcSelecying.countInventorySlot, npcSelecying.roleNpc, listItemDataInventoryEqicment);

        // Build a dictionary mapping SlotType to list of available slots
        Dictionary<SlotType, List<InvenrotySlots>> slotsByType = new Dictionary<SlotType, List<InvenrotySlots>>();

        // Initialize the dictionary
        foreach (SlotType slotType in Enum.GetValues(typeof(SlotType)))
        {
            slotsByType[slotType] = new List<InvenrotySlots>();
        }

        foreach (InvenrotySlots slot in listInvenrotySlotsUI)
        {
            slotsByType[slot.slotTypeInventory].Add(slot);
        }

        // Keep track of which slots have been used
        HashSet<InvenrotySlots> usedSlots = new HashSet<InvenrotySlots>();

        // Iterate through inventory slots (for items in listItemDataInventoryslot)
        for (int i = listItemDataInventorySlot.Count - 1; i >= 0; i--)
        {
            ItemData itemData = listItemDataInventorySlot.ElementAt(i);

            if (itemData.count == 0)
            {
                listItemDataInventorySlot.RemoveAt(i); // Remove item with count 0
            }
            else
            {
                // Find the next available slot for inventory items (Assuming these are general slots)
                // Assuming inventory slots are of type SlotBag
                List<InvenrotySlots> bagSlots = slotsByType[SlotType.SlotBag];

                // Find the next unused slot
                InvenrotySlots inventortSlot = bagSlots.FirstOrDefault(slot => !usedSlots.Contains(slot));

                if (inventortSlot != null)
                {
                    usedSlots.Add(inventortSlot);
                    GameObject uIItem = CreateUIItem(itemData, inventortSlot);
                }
                else
                {

                }
            }
        }

        // Iterate through equipment items
        for (int i = listItemDataInventoryEqicment.Count - 1; i >= 0; i--)
        {
            ItemData itemData = listItemDataInventoryEqicment.ElementAt(i);

            if (itemData.count == 0)
            {
                listItemDataInventoryEqicment.RemoveAt(i); // Remove item with count 0
            }
            else
            {
                // Determine the SlotType based on itemData.itemtype
                SlotType requiredSlotType = GetSlotTypeForItemType(itemData.itemtype);

                if (requiredSlotType != SlotType.SlotLock)
                {
                    List<InvenrotySlots> slotsOfType = slotsByType[requiredSlotType];

                    // Find the next unused slot of this type
                    InvenrotySlots inventortEqicment = slotsOfType.LastOrDefault(slot => !usedSlots.Contains(slot));

                    if (inventortEqicment != null)
                    {
                        usedSlots.Add(inventortEqicment);
                        GameObject uIItemEqicment = CreateUIItem(itemData, inventortEqicment);
                    }
                    else
                    {
                        // No available slots of this type
                        // Handle this case if needed
                    }
                }
                else
                {
                    // No valid slot type for this item
                    // Handle this case if needed
                }
            }
        }
    }
    private SlotType GetSlotTypeForItemType(Itemtype itemType)
    {
        switch (itemType)
        {
            case Itemtype.Weapon:
                return SlotType.SlotWeapon;
            case Itemtype.Vest:
                return SlotType.SlotVest;
            case Itemtype.Backpack:
                return SlotType.SlotBackpack;
            case Itemtype.Tool:
                return SlotType.SlotTool;
            case Itemtype.Grenade:
                return SlotType.SlotGrenade;
            // Add other mappings as needed
            default:
                return SlotType.SlotLock; // Indicating no valid slot
        }
    }
    public StatAmplifier statAmplifier;
    public void SelectNpcDefenseScene()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        statAmplifier = FindObjectOfType<StatAmplifier>();
        StatManager statManager = FindObjectOfType<StatManager>();

        // Update StatAmplifier properties
        statAmplifier.endurance = npcSelecying.endurance;
        statAmplifier.combat = npcSelecying.combat;
        statAmplifier.speed = npcSelecying.speed;
        statAmplifier.specialistRole = npcSelecying.roleNpc;

        // Initialize amplifiers
        statAmplifier.InitializeAmplifiers();

        // Notify StatManager of the change
        statManager.OnStatAmplifierChanged();

        // Update player's current stamina based on new max stamina
        player.currentStamina = statManager.maxStamina;

        // If you have a weapon equipped, ensure it updates the base stats
        Weapon weapon = player.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.OnStatsChanged();
        }
        npcManager.listNpc.Remove(npcSelecying);
        npcManager.listNpcWorkingMoreOneDay.Remove(npcSelecying);
        npcManager.listNpcWorkingWIthInOneDay.Remove(npcSelecying);
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
        listItemDataInventorySlot.Clear();
        listItemDataInventoryEqicment.Clear();
        ConventAllUIItemInListInventorySlotToListItemData(listItemDataInventorySlot);
        ConventAllUIItemInListInventorySlotToListEqicmentItemData(listItemDataInventoryEqicment);

        // Weapon logic
        List<ItemData> weaponItemDataList = listItemDataInventoryEqicment
        .Where(item => item.itemtype == Itemtype.Weapon)
        .ToList();

        if (weaponItemDataList.Count > 0)
        {
            List<ItemWeapon> itemWeapons = new List<ItemWeapon>();

            foreach (var weaponItemData in weaponItemDataList)
            {
                UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab
                    .FirstOrDefault(uiItem => uiItem.idItem == weaponItemData.idItem);

                if (uiItemData != null)
                {
                    ItemWeapon itemWeapon = uiItemData.GetComponent<ItemWeapon>();
                    itemWeapons.Add(itemWeapon);
                }
            }

            OnWeaponsChanged?.Invoke(itemWeapons);
        }
        else
        {
            OnWeaponsChanged?.Invoke(null);
        }

        // Vest logic
        ItemData vestItemData = listItemDataInventoryEqicment.FirstOrDefault(item => item.itemtype == Itemtype.Vest);
        if (vestItemData != null)
        {
            UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab
                .FirstOrDefault(uiItem => uiItem.idItem == vestItemData.idItem);

            if (uiItemData != null)
            {
                ItemVest itemVest = uiItemData.GetComponent<ItemVest>();
                OnVestChanged?.Invoke(itemVest);
            }
            else
            {
                OnVestChanged?.Invoke(null); // No vest
            }
        }
        else
        {
            OnVestChanged?.Invoke(null);
        }
         ItemData backpackItemData = listItemDataInventoryEqicment.FirstOrDefault(item => item.itemtype == Itemtype.Backpack);
        if (backpackItemData != null)
        {
            UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab
                .FirstOrDefault(uiItem => uiItem.idItem == backpackItemData.idItem);

            if (uiItemData != null)
            {
                ItemBackpack itemBackpack = uiItemData.GetComponent<ItemBackpack>();
                OnBackpackChanged?.Invoke(itemBackpack);
            }
            else
            {
                OnBackpackChanged?.Invoke(null); // No backpack
            }
        }
        else
        {
            OnBackpackChanged?.Invoke(null);
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

