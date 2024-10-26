using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CraftingItemUI : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI amountInInventoryText;
    public TextMeshProUGUI craftingTimeText;

    private CraftingItem craftingItemData;
    private WorkshopUI workshopUI;

    public Button buttonComponent; // Assign this in the Inspector

    public void Initialize(CraftingItem itemData, WorkshopUI parentUI)
    {
        craftingItemData = itemData;
        workshopUI = parentUI;

        if (itemIconImage != null)
            itemIconImage.sprite = itemData.itemIcon;

        if (itemNameText != null)
            itemNameText.text = itemData.itemName;

        if (amountInInventoryText != null)
            amountInInventoryText.text = "Amount: " + itemData.amountInInventory.ToString();

        if (craftingTimeText != null)
            craftingTimeText.text = "Time: " + (itemData.craftingTime / 1000f).ToString("F1") + " hr";

        // Add click listener
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        // Notify the WorkshopUI that this item was clicked
        if (workshopUI != null)
        {
            workshopUI.DisplaySelectedItemDetails(craftingItemData);
        }
    }

    private void OnDestroy()
    {
        // Clean up listener to prevent memory leaks
        if (buttonComponent != null)
        {
            buttonComponent.onClick.RemoveListener(OnClick);
        }
    }
}
