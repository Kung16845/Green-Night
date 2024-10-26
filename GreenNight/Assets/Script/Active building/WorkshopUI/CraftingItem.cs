using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CraftingItem
{
    public int itemID;
    public string itemName;
    public int amountInInventory;
    public float craftingTime; // 1000 = 1 hr
    public int rarity;
    public Sprite itemIcon;
    public List<RecipeItem> recipeItems; // Items needed to craft this item
}
[System.Serializable]
public class RecipeItem
{
    public int itemID;
    public string itemName;
    public int amountNeeded;
}
