using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SlotType uITypeItem;
    public Transform parentAfterDray;
    public Transform parentBeforeDray;
    public Image imageItem;

    // Add a reference to the ItemClass
    public ItemClass itemClass;

    // Reference to InventoryItemPresent
    private InventoryItemPresent inventoryItemPresent;
    private void Start()
    {
        inventoryItemPresent = FindAnyObjectByType<InventoryItemPresent>();
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDray = transform.parent;
        parentBeforeDray = parentAfterDray.transform;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        imageItem.raycastTarget = false;

        // Access the InventoryItemPresent instance
        inventoryItemPresent = InventoryItemPresent.Instance;

        // Ensure itemClass is assigned
        if (itemClass == null)
        {
            itemClass = GetComponent<ItemClass>();
        }

        // Check if the item is a weapon
        if (itemClass != null && itemClass is ItemWeapon)
        {
            ItemWeapon weapon = itemClass as ItemWeapon;
            Ammotype weaponAmmoType = weapon.ammoType;

            // Highlight the corresponding ammo items in the inventory
            inventoryItemPresent.HighlightAmmoItems(weaponAmmoType);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDray);
        imageItem.raycastTarget = true;

        // Reset highlighting when dragging ends
        if (inventoryItemPresent != null)
        {
            inventoryItemPresent.ResetAmmoHighlighting();
        }
        
    }
}
