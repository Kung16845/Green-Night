using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public ItemData item; // Reference to the item
    [Range(1, 100)]
    public float dropChance; // Drop chance (1-100)
}

public class LootPool : MonoBehaviour
{
    public List<LootItem> lootItems = new List<LootItem>();

    public ItemData GetRandomItem()
    {
        float totalChance = 0f;
        foreach (var lootItem in lootItems)
        {
            totalChance += lootItem.dropChance;
        }

        float randomValue = Random.Range(0, totalChance);
        float cumulativeChance = 0f;

        foreach (var lootItem in lootItems)
        {
            cumulativeChance += lootItem.dropChance;
            if (randomValue <= cumulativeChance)
            {
                return lootItem.item;
            }
        }

        return null; // In case something goes wrong
    }
}
