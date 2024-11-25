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
                return;
            }
        }

        if (uiInventory == null)
        {
            uiInventory = FindObjectOfType<UIInventory>();
            if (uiInventory == null)
            {
                return;
            }
        }

        if (inventoryItemPresent == null)
        {
            inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
            if (inventoryItemPresent == null)
            {
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
                weaponImage.enabled = false; // Hide the image if no match
            }
        }
        else
        {
            weaponImage.enabled = false; // Hide the image if no weapon is equipped
        }
    }
}
