using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemUI : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI amountInInventoryText;

    public CraftingItem craftingItemData;

    public void Initialize(CraftingItem itemData)
    {
        craftingItemData = itemData;

        if (itemIconImage != null)
            itemIconImage.sprite = itemData.itemIcon;

        // if (itemNameText != null)
        //     itemNameText.text = itemData.itemName;

        // if (amountInInventoryText != null)
        //     amountInInventoryText.text = "Amount: " + itemData.amountInInventory.ToString();
    }
}
