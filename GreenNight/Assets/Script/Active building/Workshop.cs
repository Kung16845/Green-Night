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
        if (building.isfinsih && !upgradeBuilding.isBuilding)
        {
            uImanger.ActiveWorkshopUI();
            if(upgradeBuilding.currentLevel == upgradeBuilding.maxLevel)
            {
                uImanger.DisableUpgradeworkshopButton();
            }
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
        // Get the UIItemData for the crafted item
        UIItemData uiItemData = inventoryItemPresent.listUIItemPrefab.Find(uiItem => uiItem.idItem == job.craftingItem.itemID);
        if (uiItemData == null)
        {
            Debug.LogError($"UIItemData not found for itemID: {job.craftingItem.itemID}");
            return;
        }

        // Get the ItemClass component from UIItemData
        ItemClass itemClass = uiItemData.GetComponent<ItemClass>();
        if (itemClass == null)
        {
            Debug.LogError($"ItemClass component not found on UIItemData for itemID: {job.craftingItem.itemID}");
            return;
        }

        // Calculate the total amount to add
        int totalAmount = job.craftingItem.amountProduced;
        int maxStack = itemClass.maxCountItem;

        // Loop to handle multiple stacks if needed
        while (totalAmount > 0)
        {
            // Determine how many items to add in this iteration
            int amountToAdd = Mathf.Min(totalAmount, maxStack);

            // Create ItemData with the determined amount
            ItemData craftedItemData = new ItemData
            {
                idItem = job.craftingItem.itemID,
                nameItem = itemClass.nameItem,
                count = amountToAdd,
                maxCount = itemClass.maxCountItem,
                itemtype = itemClass.itemtype,
                parantslotType = uiItemData.slotTypeParent,
                // Include any other fields as needed
            };

            // Add the crafted item to the inventory
            inventoryItemPresent.AddItem(craftedItemData);

            // Debug log for each addition
            Debug.Log($"Crafting complete: {job.craftingItem.itemName}, Amount Added: {amountToAdd}");

            // Decrease the total amount by the amount added in this iteration
            totalAmount -= amountToAdd;
        }
    }
}
