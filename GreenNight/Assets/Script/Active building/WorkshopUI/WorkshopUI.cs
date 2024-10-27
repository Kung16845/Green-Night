using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WorkshopUI : MonoBehaviour
{
    public Workshop workshop;
    public TextMeshProUGUI Craftingslot;
    public TextMeshProUGUI ActionSpeed;

    // UI elements for selected item details
    public Image selectedItemImage;
    public TextMeshProUGUI selectedItemNameText;
    public TextMeshProUGUI selectedItemCraftingTimeText;
    public Transform recipeItemsParent;
    public GameObject recipeItemUIPrefab;

    public InventoryItemPresent inventoryItemPresent;

    public GameObject craftingItemUIPrefab; // Assign in Inspector
    public Transform craftingItemsParent; // Assign in Inspector

    void Start()
    {
        workshop = FindObjectOfType<Workshop>();
        AssignActionSpeedAndSlot();
        AutoAssignCraftingItemData();
    }

    void AssignActionSpeedAndSlot()
    {
        float actionSpeedIncreasePercent = workshop.Actionspeedincrease * 100f;
        ActionSpeed.text = "Action Speed: +" + actionSpeedIncreasePercent.ToString("F0") + "%";
        int Slotincrease = workshop.Craftingslot;
        Craftingslot.text = "Crafting slot: +" + Slotincrease.ToString("F0");
    }
    void AutoAssignCraftingItemData()
    {
        // For Level 1 Items
        foreach (CraftingItem craftingItem in workshop.craftingItemsLevel1)
        {
            AutoAssignCraftingItemProperties(craftingItem);
        }

        // For Level 2 Items
        foreach (CraftingItem craftingItem in workshop.craftingItemsLevel2)
        {
            AutoAssignCraftingItemProperties(craftingItem);
        }
    }

    void AutoAssignCraftingItemProperties(CraftingItem craftingItem)
    {
        // Assign properties from InventoryItemPresent or UIItemData
        UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab.Find(uiItem => uiItem.idItem == craftingItem.itemID);

        if (uiItemData != null)
        {
            craftingItem.itemName = uiItemData.nameItem;
            craftingItem.itemIcon = uiItemData.itemIconImage.sprite;

            // If you need rarity or other properties
            ItemClass itemClass = uiItemData.GetComponent<ItemClass>();
            if (itemClass != null)
            {
                craftingItem.rarity = itemClass.rarityItem;
            }
        }
        else
        {
            Debug.LogWarning($"UIItemData not found for itemID: {craftingItem.itemID}");
        }

        // Auto-assign for each RecipeItem
        foreach (RecipeItem recipeItem in craftingItem.recipeItems)
        {
            AutoAssignRecipeItemProperties(recipeItem);
        }
    }

    void AutoAssignRecipeItemProperties(RecipeItem recipeItem)
    {
        UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab.Find(uiItem => uiItem.idItem == recipeItem.itemID);

        if (uiItemData != null)
        {
            recipeItem.itemName = uiItemData.nameItem;
            recipeItem.itemIcon = uiItemData.itemIconImage.sprite;
        }
        else
        {
            Debug.LogWarning($"UIItemData not found for itemID: {recipeItem.itemID}");
        }
    }
   void DisplayCraftingItems()
    {
        // Get the workshop's current level
        int workshopLevel = workshop.upgradeBuilding.currentLevel;

        // Get the crafting items based on the level
        List<CraftingItem> availableCraftingItems = new List<CraftingItem>();

        // Level 1 items are always included
        availableCraftingItems.AddRange(workshop.craftingItemsLevel1);

        // If level >= 2, include level 2 items
        if (workshopLevel >= 2)
        {
            availableCraftingItems.AddRange(workshop.craftingItemsLevel2);
        }

        // Order the list by rarity and then by name
        var orderedCraftingItems = availableCraftingItems.OrderBy(item => item.rarity)
                                                        .ThenBy(item => item.itemName)
                                                        .ToList();

        // Instantiate the crafting item UI prefabs
        foreach (CraftingItem craftingItem in orderedCraftingItems)
        {
            GameObject newCraftingItemUI = Instantiate(craftingItemUIPrefab, craftingItemsParent);
            CraftingItemUI craftingItemUIScript = newCraftingItemUI.GetComponent<CraftingItemUI>();
            if (craftingItemUIScript != null)
            {
                craftingItemUIScript.Initialize(craftingItem, this);
            }
        }
    }


    public void DisplaySelectedItemDetails(CraftingItem selectedItem)
    {
        if (selectedItemImage != null)
            selectedItemImage.sprite = selectedItem.itemIcon;

        if (selectedItemNameText != null)
            selectedItemNameText.text = selectedItem.itemName;

        if (selectedItemCraftingTimeText != null)
            selectedItemCraftingTimeText.text = $"{(selectedItem.craftingTime / 1000f):F1} hr";

        // Clear existing recipe items
        foreach (Transform child in recipeItemsParent)
        {
            Destroy(child.gameObject);
        }

        // Display recipe items
        foreach (RecipeItem recipeItem in selectedItem.recipeItems)
        {
            GameObject recipeItemUIObject = Instantiate(recipeItemUIPrefab, recipeItemsParent);
            RecipeItemUI recipeItemUIScript = recipeItemUIObject.GetComponent<RecipeItemUI>();

            int amountHave = inventoryItemPresent.GetItemCountByID(recipeItem.itemID);

            recipeItemUIScript.Initialize(recipeItem, amountHave);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);
        }
    }
}
