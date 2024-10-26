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
    public GameObject craftingItemUIPrefab; // Assign in Inspector
    public Transform craftingItemsParent; // Assign in Inspector (UI parent object)
    void Start()
    {
        workshop = FindObjectOfType<Workshop>();
        Assignactionspeedandslot();
    }
    public void DisplayCraftingItems()
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
                craftingItemUIScript.Initialize(craftingItem);
            }
        }
    }

    void Assignactionspeedandslot()
    {
        float actionSpeedIncreasePercent = workshop.Actionspeedincrease * 100f;
        ActionSpeed.text = "Action Speed: +" + actionSpeedIncreasePercent.ToString("F0") + "%";
        int Slotincrease = workshop.Craftingslot;
        Craftingslot.text = "Crafting slot: +" + Slotincrease.ToString("F0");
    }
    void Update()
    {
          if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);  // Disable the GameObject this script is attached to
        }
    }
}
