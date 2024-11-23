using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class WeaponImageDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public Image weaponImage; // UI Image to display the weapon's icon

    [Header("References")]
    public Weapon weapon; // Reference to the Weapon script
    public UIInventory uiInventory; // Reference to the inventory system
    public InventoryItemPresent inventoryItemPresent; // Reference to manage item data and UI updates

    private void Start()
    {
        // Find references if not assigned
        if (weapon == null)
        {
            weapon = FindObjectOfType<Weapon>();
            if (weapon == null)
            {
                Debug.LogError("Weapon reference not found!");
                return;
            }
        }

        if (uiInventory == null)
        {
            uiInventory = FindObjectOfType<UIInventory>();
            if (uiInventory == null)
            {
                Debug.LogError("UIInventory not found!");
                return;
            }
        }

        if (inventoryItemPresent == null)
        {
            inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
            if (inventoryItemPresent == null)
            {
                Debug.LogError("InventoryItemPresent not found!");
                return;
            }
        }

        UpdateWeaponImage();
    }

    private void Update()
    {
        UpdateWeaponImage();
    }

    private void UpdateWeaponImage()
    {
        // Get the equipped weapon item from the inventory
        var weaponItemData = uiInventory.listItemDataInventoryEqicment
            .FirstOrDefault(item => item.itemtype == Itemtype.Weapon);

        if (weaponItemData != null)
        {
            // Find the UIItemData corresponding to the weapon item
            var uiItemData = inventoryItemPresent.listUIItemPrefab
                .FirstOrDefault(uiItem => uiItem.idItem == weaponItemData.idItem);

            if (uiItemData != null)
            {
                // Update the weapon image with the sprite
                weaponImage.sprite = uiItemData.itemIconImage.sprite;
                weaponImage.enabled = true; // Ensure the image is visible
            }
            else
            {
                Debug.LogWarning($"UIItemData not found for item ID: {weaponItemData.idItem}");
                weaponImage.enabled = false; // Hide the image if no match
            }
        }
        else
        {
            Debug.LogWarning("No weapon item equipped.");
            weaponImage.enabled = false; // Hide the image if no weapon is equipped
        }
    }
}
