using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class UIInventory : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public NpcManager npcManager;
    public NpcClass npcSelecying;
    public List<InvenrotySlots> listInvenrotySlots = new List<InvenrotySlots>();
    public InventoryItemPresent inventoryItemPresent;
    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI levelEnduranceText;
    public TextMeshProUGUI levelCombatText;
    public TextMeshProUGUI levelSpeedText; 
    public TextMeshProUGUI specialistNpcText; 
    private void Awake()
    {
        SetValuableUIInventory();
    }
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
    public void SelectNpcDefenseScene()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.currentSpeed = npcSelecying.speed;

        StatAmplifier statAmplifier = FindObjectOfType<StatAmplifier>();

        statAmplifier.endurance = npcSelecying.endurance;
        statAmplifier.combat = npcSelecying.combat;
        statAmplifier.speed = npcSelecying.speed;
        
        // npcManager.listNpc.Remove(npcSelecying);

    }
    private void OnDestroy()
    {
        ClearItemDataInAllInventorySlot();
    }
}

