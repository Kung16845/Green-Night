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
    }

    void AssignActionSpeedAndSlot()
    {
        float actionSpeedIncreasePercent = workshop.Actionspeedincrease * 100f;
        ActionSpeed.text = "Action Speed: +" + actionSpeedIncreasePercent.ToString("F0") + "%";
        int Slotincrease = workshop.Craftingslot;
        Craftingslot.text = "Crafting slot: +" + Slotincrease.ToString("F0");
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
            selectedItemCraftingTimeText.text = $"Crafting Time: {(selectedItem.craftingTime / 1000f).ToString("F1")} hr";

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

            // Use methods from InventoryItemPresent
            int amountHave = inventoryItemPresent.GetItemCountByID(recipeItem.itemID);
            Sprite itemIcon = inventoryItemPresent.GetItemIconByID(recipeItem.itemID);

            recipeItemUIScript.Initialize(recipeItem, amountHave, itemIcon);
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
