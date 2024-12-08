using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScriptMoveItems : MonoBehaviour
{
    public int countItemMove = 1;
    public TextMeshProUGUI countText;
    public ItemClass itemClassMove;
    public LootingSystem originatingLootSystem;
    public ItemClass itemClassInChild;
    public DraggableItem draggableItemMove;
    public InventoryItemPresent inventoryItemPresent;
    public UIInventory uIInventory;
    // Start is called before the first frame update
    private void OnEnable()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        uIInventory = FindObjectOfType<UIInventory>();
        // Debug.Log("Open UI ScriptMoveItens");
    }
    void Start()
    {
        uIInventory = FindObjectOfType<UIInventory>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        countItemMove = 1;
        countText.text = countItemMove.ToString();

    }
    public void IncreateCountItem(int count)
    {
        countItemMove += count;
        SlotType slotTypeItemMove = itemClassMove.gameObject.GetComponentInParent<InvenrotySlots>().slotTypeInventory;

        if (slotTypeItemMove == SlotType.SlotLoot)
        {
            // Handle count increase for loot system
            LootingSystem lootSystem = itemClassMove.GetComponent<UIItemData>().originatingLootSystem;

            if (lootSystem != null)
            {
                ItemData lootItem = lootSystem.droppedItems.FirstOrDefault(d => d.idItem == itemClassMove.idItem);
                if (lootItem != null)
                {
                    // Limit countItemMove to the available loot item quantity
                    if (countItemMove > lootItem.count)
                    {
                        countItemMove = lootItem.count;
                    }
                }
            }
        }

        if (itemClassInChild == null)
        {
            // Check if countItemMove exceeds maxCountItem or available quantity
            if (countItemMove > itemClassMove.quantityItem)
            {
                countItemMove = itemClassMove.quantityItem;
            }
            if (countItemMove > itemClassMove.maxCountItem)
            {
                countItemMove = itemClassMove.maxCountItem;
            }
        }
        else if (itemClassInChild != null)
        {
            // Ensure the total quantity in child and move does not exceed maxCountItem
            int totalQuantity = itemClassInChild.quantityItem + countItemMove;

            if (totalQuantity > itemClassMove.quantityItem)
            {
                countItemMove = itemClassMove.quantityItem - itemClassInChild.quantityItem;
            }
            if (totalQuantity > itemClassMove.maxCountItem)
            {
                countItemMove = itemClassMove.maxCountItem - itemClassInChild.quantityItem;
            }
        }
        countText.text = countItemMove.ToString();
    }

    public void DecreasteCountItem(int count)
    {
        countItemMove -= count;

        // Ensure countItemMove does not fall below 1
        if (countItemMove < 1)
        {
            countItemMove = 1;
        }

        SlotType slotTypeItemMove = itemClassMove.gameObject.GetComponentInParent<InvenrotySlots>().slotTypeInventory;

        if (slotTypeItemMove == SlotType.SlotLoot)
        {
            // Handle count decrease for loot system
            LootingSystem lootSystem = itemClassMove.GetComponent<UIItemData>().originatingLootSystem;

            if (lootSystem != null)
            {
                ItemData lootItem = lootSystem.droppedItems.FirstOrDefault(d => d.idItem == itemClassMove.idItem);
                if (lootItem != null)
                {
                    if (countItemMove > lootItem.count)
                    {
                        countItemMove = lootItem.count;
                    }
                }
            }
        }

        if (countItemMove == 1)
        {
            if (itemClassInChild == null)
            {
                countItemMove = Mathf.Min(itemClassMove.maxCountItem, itemClassMove.quantityItem);
            }
            else if (itemClassInChild != null)
            {
                int maxAllowed = itemClassMove.maxCountItem - itemClassInChild.quantityItem;
                countItemMove = Mathf.Min(itemClassMove.quantityItem, maxAllowed);
            }
        }
        else
        {
            if (itemClassInChild == null)
            {
                if (countItemMove > itemClassMove.quantityItem)
                {
                    countItemMove = itemClassMove.quantityItem;
                }
            }
            else if (itemClassInChild != null)
            {
                int maxAllowed = itemClassMove.maxCountItem - itemClassInChild.quantityItem;

                if (countItemMove > itemClassMove.quantityItem || countItemMove > maxAllowed)
                {
                    countItemMove = Mathf.Min(itemClassMove.quantityItem, maxAllowed);
                }
            }
        }

        countText.text = countItemMove.ToString();
    }
    public void MoveItem()
    {
        // Move Item from boxes to inventory slot
        List<ItemData> listItemDataBox = inventoryItemPresent.listItemsDataBox;
        ItemData itemData = listItemDataBox.FirstOrDefault(item => item.idItem == itemClassMove.idItem);
        SlotType slotTypeItemMoveParantBefore = draggableItemMove.parentBeforeDray.GetComponent<InvenrotySlots>().slotTypeInventory;

        if (slotTypeItemMoveParantBefore == SlotType.SlotBoxes && itemClassInChild == null)
        {
            itemData.count -= countItemMove;
            itemClassMove.quantityItem = countItemMove;
            UpdateUIItemMove();
        }
        else if (slotTypeItemMoveParantBefore == SlotType.SlotLoot && itemClassMove != null && itemClassInChild == null)
        {
            // Access the loot system from the item's originating data
            LootingSystem lootSystem = itemClassMove.GetComponent<UIItemData>().originatingLootSystem;

            if (lootSystem != null)
            {
                // Find the item in the loot system
                ItemData lootItem = lootSystem.droppedItems.FirstOrDefault(d => d.idItem == itemClassMove.idItem);
                if (lootItem != null)
                {
                    // Ensure only countItemMove amount is moved
                    if (lootItem.count >= countItemMove)
                    {
                        lootItem.count -= countItemMove; // Reduce count in the loot system
                        itemClassMove.quantityItem = countItemMove; // Set the moved item's quantity
                    }
                    else
                    {
                        // Move the remaining quantity if less than countItemMove
                        itemClassMove.quantityItem = lootItem.count;
                        lootItem.count = 0;
                    }

                    Debug.Log($"Moved {itemClassMove.quantityItem} items from loot. Remaining in loot: {lootItem.count}");

                    // Remove the item from loot if its count is zero
                    if (lootItem.count <= 0)
                    {
                        lootSystem.droppedItems.Remove(lootItem);
                    }
                }
            }

            // Update the UI to reflect the new item count
            UpdateUIItemMove();
        }

        else if (slotTypeItemMoveParantBefore == SlotType.SlotLoot && itemClassInChild != null)
        {
            // Access the originating loot system
            LootingSystem lootSystem = itemClassMove.GetComponent<UIItemData>().originatingLootSystem;

            if (lootSystem != null)
            {
                // Find the item in the loot system
                ItemData lootItem = lootSystem.droppedItems.FirstOrDefault(d => d.idItem == itemClassMove.idItem);
                if (lootItem != null)
                {
                    // Calculate the actual amount that can be moved based on the loot and child slot capacity
                    int availableToMove = Mathf.Min(countItemMove, lootItem.count);
                    int availableSpaceInChild = itemClassInChild.maxCountItem - itemClassInChild.quantityItem;
                    int actualMoveAmount = Mathf.Min(availableToMove, availableSpaceInChild);

                    // Reduce the count in the loot system and transfer the actual move amount
                    lootItem.count -= actualMoveAmount;
                    itemClassMove.quantityItem -= actualMoveAmount;
                    itemClassInChild.quantityItem += actualMoveAmount;

                    // Update loot system if the item's count reaches zero
                    if (lootItem.count <= 0)
                    {
                        lootSystem.droppedItems.Remove(lootItem);
                    }

                    // Update the child item's UI
                    GameObject uIItemInChildObject = itemClassInChild.gameObject;
                    UIItemData uIItemDataInChild = uIItemInChildObject.GetComponent<UIItemData>();
                    uIItemDataInChild.UpdateDataUI(itemClassInChild);

                    // Update the moving item's UI or destroy it if its quantity is now zero
                    if (itemClassMove.quantityItem > 0)
                    {
                        UpdateUIItemMove();
                    }
                    else
                    {
                        Destroy(itemClassMove.gameObject);
                    }

                    Debug.Log($"Moved {actualMoveAmount} items to child. Remaining in loot: {lootItem.count}, Remaining in move: {itemClassMove.quantityItem}");
                }
            }
        }
        else if (itemClassInChild != null)
        {
            itemClassMove.quantityItem -= countItemMove;
            itemClassInChild.quantityItem += countItemMove;

            if (itemClassMove.gameObject.GetComponentInParent<InvenrotySlots>().slotTypeInventory == SlotType.SlotBoxes)
            {
                itemData.count -= countItemMove;
            }

            GameObject uIItemInChildObject = itemClassInChild.gameObject;
            UIItemData uIItemDataInChild = uIItemInChildObject.GetComponent<UIItemData>();
            uIItemDataInChild.UpdateDataUI(itemClassInChild);

            if (itemClassMove.quantityItem > 0)
            {
                UpdateUIItemMove();
            }
            else
            {
                Destroy(itemClassMove.gameObject);
            }
        }

        bool isBackpackMoved = false;

        if (itemClassMove != null && itemClassMove.itemtype == Itemtype.Backpack)
        {
            isBackpackMoved = true;
        }
        else if (itemClassInChild != null && itemClassInChild.itemtype == Itemtype.Backpack)
        {
            isBackpackMoved = true;
        }

        uIInventory.ConventDataUIToItemData();

        if (itemData != null)
        {
            if (itemData.count <= 0)
            {
                listItemDataBox.Remove(itemData);
            }
        }

        if (itemClassMove.quantityItem <= 0)
        {
            Destroy(itemClassMove.gameObject);
        }

        if (isBackpackMoved)
        {
            uIInventory.RefreshUIInventory();
        }

        itemClassInChild = null;
        itemClassMove = null;
        inventoryItemPresent.RefreshUIBox();
        uIInventory.RefreshUIBoxCategory(uIInventory.currentNumCategory);
        gameObject.SetActive(false);
    }

    public void UpdateUIItemMove()
    {
        GameObject uIItemObject = itemClassMove.gameObject;
        UIItemData uIItemData = uIItemObject.GetComponent<UIItemData>();
        uIItemData.slotTypeParent = uIItemData.GetComponentInParent<InvenrotySlots>().slotTypeInventory;
        uIItemData.UpdateDataUI(itemClassMove);
    }
    public void CancleMove()
    {

        DraggableItem draggableItemMove = itemClassMove.gameObject.GetComponent<DraggableItem>();
        draggableItemMove.transform.SetParent(draggableItemMove.parentBeforeDray);
        draggableItemMove.parentAfterDray = draggableItemMove.parentBeforeDray;
        if (draggableItemMove.parentBeforeDray == inventoryItemPresent.transformsBoxes)
        {
            Destroy(draggableItemMove.gameObject);
        }
        inventoryItemPresent.ResetAmmoHighlighting();
        gameObject.SetActive(false);
    }
}
