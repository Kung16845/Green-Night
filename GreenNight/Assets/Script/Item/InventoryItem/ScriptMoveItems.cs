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
    public ItemClass itemClassInChild;
    public DraggableItem draggableItemMove;
    public InventoryItemPresent inventoryItemPresent;
    public UIInventory uIInventory;
    // Start is called before the first frame update
    private void OnEnable()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
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
        if (itemClassInChild == null)
        {
            // Debug.Log("itemClassMove.quantityItem : " + itemClassMove.quantityItem);
            // Debug.Log("itemClassInChild == null");
            if (countItemMove > itemClassMove.maxCountItem)
            {
                countItemMove = itemClassMove.maxCountItem;

                if (countItemMove > itemClassMove.quantityItem)
                {
                    countItemMove = itemClassMove.quantityItem;
                }
            }
        }
        else if (itemClassInChild != null)
        {
            // Debug.Log("item classInChild is not null ");

            if (countItemMove > itemClassMove.quantityItem)
            {
                countItemMove = itemClassMove.quantityItem;
                if (itemClassInChild.quantityItem + countItemMove > itemClassMove.maxCountItem)
                {
                    countItemMove = itemClassMove.maxCountItem - itemClassInChild.quantityItem;
                }
            }
            else if (itemClassInChild.quantityItem + countItemMove > itemClassMove.maxCountItem)
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

        if (countItemMove == 1)
        {
            // Case when countItemMove is set to 1, we allow it to reach the maximum count possible
            if (itemClassInChild == null)
            {
                // If no item exists in the child slot
                countItemMove = Mathf.Min(itemClassMove.maxCountItem, itemClassMove.quantityItem);
            }
            else if (itemClassInChild != null)
            {
                // If an item exists in the child slot
                int maxAllowed = itemClassMove.maxCountItem - itemClassInChild.quantityItem;
                countItemMove = Mathf.Min(itemClassMove.quantityItem, maxAllowed);
            }
        }
        else
        {
            // Adjust countItemMove to ensure it does not exceed limits
            if (itemClassInChild == null)
            {
                // Case when there's no item in the child slot
                if (countItemMove > itemClassMove.quantityItem)
                {
                    countItemMove = itemClassMove.quantityItem;
                }
            }
            else if (itemClassInChild != null)
            {
                // Case when an item exists in the child slot
                int maxAllowed = itemClassMove.maxCountItem - itemClassInChild.quantityItem;

                if (countItemMove > itemClassMove.quantityItem || countItemMove > maxAllowed)
                {
                    countItemMove = Mathf.Min(itemClassMove.quantityItem, maxAllowed);
                }
            }
        }

        // Update the text to reflect the new count
        countText.text = countItemMove.ToString();
    }



    public void MoveItem()
    {
        //Move Item from boxes to inventoryslot
        List<ItemData> listItemDataBox = inventoryItemPresent.listItemsDataBox;
        ItemData itemData = listItemDataBox.FirstOrDefault(item => item.idItem == itemClassMove.idItem);
        SlotType slotTypeItemMoveParantBefore = draggableItemMove.parentBeforeDray.GetComponent<InvenrotySlots>().slotTypeInventory;
        //Move Item within InventorySlot
        List<ItemData> listItemDataBoxes = inventoryItemPresent.listItemsDataBox;
        if (slotTypeItemMoveParantBefore == SlotType.SlotBoxes && itemClassInChild == null)
        {
            itemData.count -= countItemMove;
            itemClassMove.quantityItem = countItemMove;
            UpdateUIItemMove();

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

            Debug.Log("Not Parant Slot is SlotBoxes");
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
            // Refresh the inventory UI to reflect the new slots
            uIInventory.RefreshUIInventory();
        }
        itemClassInChild = null;
        itemClassMove = null;
        inventoryItemPresent.RefreshUIBox();
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
        if(draggableItemMove.parentBeforeDray == inventoryItemPresent.transformsBoxes){
            Destroy(draggableItemMove.gameObject);
        }
        inventoryItemPresent.ResetAmmoHighlighting();
        gameObject.SetActive(false);
    }
}
