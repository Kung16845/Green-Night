using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class InvenrotySlots : MonoBehaviour, IDropHandler
{
    public SlotType slotTypeInventory;
    public GameObject uIMoveItemsBoxesToInventory;
    // public GameObject uIInventoryBoxes;
    public Canvas canvas;
    public int maxCountItems;
    public InventoryItemPresent inventoryItemPresent;
    public UIInventory uIInventory;
    // Start is called before the first frame update
  
    void Start()
    {
        // Find canvas and inventory item presenter in the active scene
        canvas = FindObjectOfType<Canvas>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

        // Get all objects of type ScriptMoveItems
        if (uIMoveItemsBoxesToInventory == null)
        {
            ScriptMoveItems[] objectsWithScript = Resources.FindObjectsOfTypeAll<ScriptMoveItems>();

            // Get the active scene
            Scene activeScene = SceneManager.GetActiveScene();

            foreach (var item in objectsWithScript)
            {
                GameObject obj = item.gameObject;

                // Ensure the object is in the active scene and is not inactive
                if (obj.scene == activeScene && !obj.activeInHierarchy)
                {
                    uIMoveItemsBoxesToInventory = obj;
                    ScriptMoveItems scriptMoveItems = uIMoveItemsBoxesToInventory.GetComponent<ScriptMoveItems>();
                    scriptMoveItems.uIInventory = uIInventory;
                    break; // Optional: Stop after finding the first match
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // GameObject being dragged
        GameObject uIitem = eventData.pointerDrag;
        DraggableItem draggableItem = uIitem.GetComponent<DraggableItem>();
        UIItemData uIItemDataDrag = uIitem.GetComponent<UIItemData>();
        ItemClass itemClassMove = uIitem.GetComponent<ItemClass>();

        // If there's already an item in this slot
        UIItemData uIItemDataInChild = GetComponentInChildren<UIItemData>();
        ItemClass itemClassInChild = GetComponentInChildren<ItemClass>();

        // Script responsible for moving items
        ScriptMoveItems scriptMoveItems = uIMoveItemsBoxesToInventory.GetComponent<ScriptMoveItems>();
        
        if (slotTypeInventory == SlotType.SlotLock)
        {

            return; // Can't drop into a locked slot
        }

        // If slot is empty and matches the item type or is a bag slot
        if ((slotTypeInventory == SlotType.SlotBag || slotTypeInventory == draggableItem.uITypeItem) && transform.childCount == 0)
        {
            draggableItem.parentAfterDray = transform;
            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();

            InvenrotySlots originSlot = draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>();
            if (originSlot.slotTypeInventory == SlotType.SlotBoxes || originSlot.slotTypeInventory == SlotType.SlotLoot)
            {
                OpenUIMoveITems(scriptMoveItems);
            }
        }
        else if (slotTypeInventory == SlotType.SlotCar)
        {
            if (transform.childCount == 0) // Slot is empty
            {
                draggableItem.parentAfterDray = transform;
                scriptMoveItems.itemClassMove = itemClassMove;
                scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();

                // Check if the item is being moved from another car slot
                InvenrotySlots originSlot = draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>();
                if (originSlot.slotTypeInventory == SlotType.SlotCar)
                {
                    // Handle moving items within car slots (no special handling required here)
                    return;
                }

                // Open UI for managing item transfer if coming from another inventory
                OpenUIMoveITems(scriptMoveItems);
            }
            else if (itemClassInChild != null && uIItemDataDrag.idItem == uIItemDataInChild.idItem)
            {
                // Stacking items in the same car slot
                scriptMoveItems.itemClassMove = itemClassMove;
                scriptMoveItems.itemClassInChild = itemClassInChild;
                scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();

                OpenUIMoveITems(scriptMoveItems);
            }
            return; // End processing for car slot
        }
        else if (itemClassInChild != null && uIItemDataDrag.idItem == uIItemDataInChild.idItem && slotTypeInventory != SlotType.SlotBoxes
                && itemClassInChild.quantityItem < itemClassInChild.maxCountItem)
        {
            // Stacking items of the same type if not in boxes
            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.itemClassInChild = itemClassInChild;
            scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();

            OpenUIMoveITems(scriptMoveItems);
        }
        else if (slotTypeInventory == SlotType.SlotBoxes)
        {

            // Ensure proper checks for origin and destination slots
            InvenrotySlots originSlot = draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>();
            List<ItemData> listItemDataBoxes = inventoryItemPresent.listItemsDataBox;
            ItemData itemData = listItemDataBoxes.FirstOrDefault(item => item.idItem == itemClassMove.idItem);

            if (itemData != null && originSlot.slotTypeInventory != SlotType.SlotBoxes)
            {
                // Increment count only when moving from non-box to box
                itemData.count += itemClassMove.quantityItem;
            }
            else if (itemData == null)
            {
                ItemData newItemData = inventoryItemPresent.ConventItemClassToItemData(itemClassMove);
                inventoryItemPresent.AddItem(newItemData);
            }

            // Destroy the dragged item's GameObject only after handling its data
            Destroy(itemClassMove.gameObject);
        }

        else if (slotTypeInventory == SlotType.SlotLoot)
        {
            // Dropping into another loot slot
            InvenrotySlots originSlot = draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>();
            if (originSlot.slotTypeInventory == SlotType.SlotLoot)
            {
                // Item moved from one loot slot to another loot slot, do nothing special
                return;
            }
        }
        else if (slotTypeInventory == null)
        {
            inventoryItemPresent.RefreshUIBox();
        }

        // At this point, we've handled the main cases. Now handle the scenario:
        // If the item originated from a SlotLoot and is now placed in a non-loot slot, remove it from droppedItems.
        {
            InvenrotySlots originSlot = draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>();
            if (originSlot != null && originSlot.slotTypeInventory == SlotType.SlotLoot && slotTypeInventory != SlotType.SlotLoot)
            {
                // Item moved out of loot into a different slot type
                LootingSystem lootSystem = FindObjectOfType<LootingSystem>();
                if (lootSystem != null)
                {
                    ItemData lootItem = lootSystem.droppedItems.FirstOrDefault(d => d.idItem == itemClassMove.idItem);
                    if (lootItem != null)
                    {
                        // Reduce the count based on how many were moved
                        lootItem.count -= itemClassMove.quantityItem;
                        inventoryItemPresent.RefreshUIBox();
                        if (lootItem.count <= 0)
                        {
                            lootSystem.droppedItems.Remove(lootItem);
                        }
                    }
                }
            }
        }

        uIItemDataDrag.slotTypeParent = slotTypeInventory;

        // Refresh the UI after changes
        inventoryItemPresent.RefreshUIBox();
        inventoryItemPresent.RefreshUIBox();

    }


    public void OpenUIMoveITems(ScriptMoveItems scriptMoveItems)
    {
        Debug.Log("OpenUIMoveITems");
        scriptMoveItems.countItemMove = 1;
        scriptMoveItems.countText.text = "1";
        uIMoveItemsBoxesToInventory.SetActive(true);
        uIMoveItemsBoxesToInventory.SetActive(true);

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera, // กล้องที่ใช้ใน Canvas (ถ้าเป็น World Space)
            out mousePos);

        uIMoveItemsBoxesToInventory.GetComponent<RectTransform>().anchoredPosition = mousePos;

    }
}
public enum SlotType
{
    SlotWeapon,
    SlotVest,
    SlotTool,
    SlotBackpack,
    SlotGrenade,
    SlotBag,
    SlotLock,
    SlotBoxes,
    SlotLoot,
    SlotCar
}