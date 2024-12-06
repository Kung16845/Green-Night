using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public ItemData item; // Reference to the item
    public float dropChance; // Base drop chance (e.g., 1-100)
    public int minAmount = 1; // Minimum quantity of this item
    public int maxAmount = 1; // Maximum quantity of this item

    // Optional: If you want to make higher quantities harder to drop,
    // you could define some logic here or in the LootPool that modifies
    // the effective drop chance based on the min/max range. For example:
    //
    // public float GetAdjustedChance()
    // {
    //     // A simple formula: 
    //     // For bigger maxAmount, reduce overall chance slightly.
    //     // This is just an example. Adjust as you see fit.
    //     float difficultyMultiplier = 1f / Mathf.Max(1, maxAmount);
    //     return dropChance * difficultyMultiplier;
    // }
}

public class LootPool : MonoBehaviour
{
    public List<LootItem> lootItems = new List<LootItem>();

    // Struct to return both the chosen item and the quantity
    public struct LootResult
    {
        public ItemData item;
        public int amount;
    }

    public LootResult GetRandomLoot()
    {
        // First, calculate the total adjusted drop chance.
        float totalChance = 0f;
        foreach (var lootItem in lootItems)
        {
            // Use the base drop chance or an adjusted one if desired
            totalChance += lootItem.dropChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        // Determine which item is selected based on weighted chances.
        foreach (var lootItem in lootItems)
        {
            cumulativeChance += lootItem.dropChance;
            if (randomValue <= cumulativeChance)
            {
                // Once we select the item, choose a random amount
                int amount = Random.Range(lootItem.minAmount, lootItem.maxAmount + 1);

                // Optional: If you want to further reduce the likelihood of higher amounts,
                // you could roll again or modify the chance dynamically.
                //
                // For simplicity, just return a random amount in the given range.

                return new LootResult { item = lootItem.item, amount = amount };
            }
        }

        // In case something goes wrong, return a default result.
        return new LootResult { item = null, amount = 0 };
    }
}