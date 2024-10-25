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
    public NpcManager npcManager;
    public NpcClass npcSelecying;
    public List<InvenrotySlots> listInvenrotySlotsUI = new List<InvenrotySlots>();
    public Transform transformBoxes;
    public InventoryItemPresent inventoryItemPresent;
    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI levelEnduranceText;
    public TextMeshProUGUI levelCombatText;
    public TextMeshProUGUI levelSpeedText;
    public TextMeshProUGUI specialistNpcText;

    public void SetValuableUIInventory()
    {   
    
        npcManager = FindObjectOfType<NpcManager>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

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
    public void ClearItemDataInAllInventorySlotToListDataBoxes()
    {
        foreach (InvenrotySlots slotsItem in listInvenrotySlotsUI)
        {
            ItemClass itemClass = slotsItem.GetComponentInChildren<ItemClass>();
            if (itemClass != null)
            {
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                inventoryItemPresent.AddItem(itemData);
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
            if(itemClass != null)
            {   
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                listSlotItemDatas.Add(itemData);
            }
        }
    }
    public void ConventAllUIItemInListInventorySlotToListEqicmentItemData(List<ItemData> listEqicmentItemDatas)
    {
        for (int i = 12; i < 19; i++)
        {
            ItemClass itemClass = listInvenrotySlotsUI.ElementAt(i).GetComponentInChildren<ItemClass>();
            if(itemClass != null)
            {   
                ItemData itemData = inventoryItemPresent.ConventItemClassToItemData(itemClass);
                listEqicmentItemDatas.Add(itemData);
            }
        }
    }
    public GameObject CreateUIItem(ItemData itemData,InvenrotySlots invenrotySlots)
    {   
        // List<UIItemData> listUIItemPrefab = ;
        GameObject itemUI = inventoryItemPresent.listUIItemPrefab.FirstOrDefault(idItem => idItem.idItem == itemData.idItem).gameObject;
        Instantiate(itemUI,invenrotySlots.transform,true);

        UIItemData uIItemData = itemUI.GetComponent<UIItemData>();
        ItemClass itemClass = itemUI.GetComponent<ItemClass>();

        itemClass.quantityItem = itemData.count;
        itemClass.maxCountItem = itemData.maxCount;

        uIItemData.slotTypeParent = invenrotySlots.slotTypeInventory;
        uIItemData.UpdateDataUI(itemClass);

        return itemUI;
    }
}

