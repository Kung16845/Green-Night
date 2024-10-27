using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CraftingResult
{
    Success,
    NotEnoughItems,
    NoAvailableSlots
}
public class Workshop : MonoBehaviour
{
    public float Actionspeedincrease;
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeUi upgradeUi;
    public UpgradeBuilding upgradeBuilding;
    public UImanger uImanger;
    public Globalstat globalstat;
    public int Craftingslot;
    public bool Isapplyspeed;
    public List<CraftingItem> craftingItemsLevel1;
    public List<CraftingItem> craftingItemsLevel2;
    public int maxCraftingSlots = 3;
    public List<CraftingJob> activeCraftingJobs = new List<CraftingJob>();

    public InventoryItemPresent inventoryItemPresent;

    void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        uImanger = FindObjectOfType<UImanger>();
        timeManager = FindObjectOfType<TimeManager>();
        globalstat = FindObjectOfType<Globalstat>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
        Actionspeedincrease = 0.25f;
        Craftingslot = 3;
        Isapplyspeed = false;
    }

    void Update()
    {
        IsElectricActive();
        IsElectricInactive();
        UpdateCraftingJobs();
    }

    void OnMouseDown()
    {
        if (building.isfinsih && !upgradeBuilding.isBuilding && upgradeBuilding.currentLevel < upgradeBuilding.maxLevel)
        {
            uImanger.ActiveWorkshopUI();
            AssignUpgradeData();
        }
    }
    private void AssignUpgradeData()
    {
        uImanger.ActiveUpgradeUI();
        upgradeUi = FindObjectOfType<UpgradeUi>();
        upgradeUi.Initialize(upgradeBuilding);
        uImanger.DisableUpgradeUI();
    }
    void IsElectricActive()
    {
        if (building.isfinsih && buildManager.iselecticitiesactive)
        {
            float IncreaseActionSpeed = 0.25f;
            if (!Isapplyspeed)
            {
                globalstat.CalculateActionSpeed(IncreaseActionSpeed);
                Isapplyspeed = true;
            }
        }
    }

    void IsElectricInactive()
    {
        if (building.isfinsih && !buildManager.iselecticitiesactive)
        {
            float DecreaseActionSpeed = 0.25f;
            if (Isapplyspeed)
            {
                globalstat.CalculateActionSpeed(-DecreaseActionSpeed);
                Isapplyspeed = false;
            }
        }
    }
    public CraftingResult AddCraftingJob(CraftingItem craftingItem)
    {
        if (activeCraftingJobs.Count >= maxCraftingSlots)
        {
            Debug.LogWarning("No available crafting slots.");
            return CraftingResult.NoAvailableSlots;
        }

        // Check if the player has enough items for the recipe
        foreach (RecipeItem recipeItem in craftingItem.recipeItems)
        {
            int amountHave = inventoryItemPresent.GetItemCountByID(recipeItem.itemID);
            if (amountHave < recipeItem.amountNeeded)
            {
                Debug.LogWarning($"Not enough {recipeItem.itemName}. Required: {recipeItem.amountNeeded}, Have: {amountHave}");
                return CraftingResult.NotEnoughItems; // Not enough items
            }
        }

        // Remove required items from inventory
        foreach (RecipeItem recipeItem in craftingItem.recipeItems)
        {
            ItemData itemDataToRemove = new ItemData
            {
                idItem = recipeItem.itemID,
                count = recipeItem.amountNeeded
            };
            inventoryItemPresent.RemoveItem(itemDataToRemove);
        }

        // Add the crafting job
        CraftingJob newJob = new CraftingJob(craftingItem);
        activeCraftingJobs.Add(newJob);
        return CraftingResult.Success;
    }

    void UpdateCraftingJobs()
    {
        for (int i = activeCraftingJobs.Count - 1; i >= 0; i--)
        {
            CraftingJob job = activeCraftingJobs[i];
            if (!job.isComplete)
            {
                job.timeRemaining -= Time.deltaTime;

                if (job.timeRemaining <= 0f)
                {
                    job.timeRemaining = 0f;
                    job.isComplete = true;
                    CompleteCraftingJob(job);
                    activeCraftingJobs.RemoveAt(i);
                }
            }
        }
    }

    void CompleteCraftingJob(CraftingJob job)
    {
        // Add the crafted item to the inventory
        ItemData craftedItemData = new ItemData
        {
            idItem = job.craftingItem.itemID,
            count = 1 // Adjust quantity as needed
            // nameItem = job.craftingItem.itemName,
            // maxCount,
            // Itemtype itemtype,
            // SlotType parantslotType
        };
        inventoryItemPresent.AddItem(craftedItemData);

        Debug.Log($"Crafting complete: {job.craftingItem.itemName}");
    }
}
