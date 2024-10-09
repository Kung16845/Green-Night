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
    // Start is called before the first frame update
    void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        countItemMove = 1;
        countText.text = countItemMove.ToString();
    }
    public void IncreateCountItem(int count)
    {
        countItemMove += count;
        SlotType slotTypeItemMove = itemClassMove.gameObject.GetComponentInParent<InvenrotySlots>().slotTypeInventory;
        if (itemClassInChild == null )
        {   
            Debug.Log("itemClassInChild == null");
            if (countItemMove > itemClassMove.quantityItem && itemClassMove.quantityItem < itemClassMove.maxCountItem)
            {   
                Debug.Log("countItemMove = itemClassMove.quantityItem && itemClassMove.quantityItem < itemClassMove.maxCountItem");
                countItemMove = itemClassMove.quantityItem;  
            }
            else if(countItemMove < itemClassMove.quantityItem && itemClassMove.quantityItem  > itemClassMove.maxCountItem)
            {   
                Debug.Log("countItemMove < itemClassMove.quantityItem &x& itemClassMove.quantityItem  > itemClassMove.maxCountItem");
             
                countItemMove = itemClassMove.maxCountItem;
            }

        }
        else if (itemClassInChild != null)
        {
            Debug.Log("item classInChild is not null ");
            if (slotTypeItemMove != SlotType.SlotBoxes)
            {
                if (countItemMove > itemClassMove.quantityItem)
                {
                    countItemMove = itemClassMove.quantityItem;

                    if (itemClassInChild.quantityItem + countItemMove > itemClassMove.maxCountItem)
                    {
                        countItemMove = itemClassMove.maxCountItem - itemClassInChild.quantityItem;
                    }
                }
            }
            else 
            {
                if(countItemMove > itemClassMove.quantityItem)
                {
                    countItemMove = itemClassMove.quantityItem;
                }
            }
        }

        countText.text = countItemMove.ToString();
    }
    public void DecreasteCountItem(int count)
    {
        countItemMove -= count;
        if (countItemMove < 1)
        {
            countItemMove = 1;
        }
        countText.text = countItemMove.ToString();
    }
    public void MoveItem()
    {


        List<ItemData> listItemData = inventoryItemPresent.listItemsDataBox;
        ItemData itemData = listItemData.FirstOrDefault(item => item.idItem == itemClassMove.idItem);
        SlotType slotTypeItemMoveParantBefore = draggableItemMove.parentBeforeDray.GetComponent<InvenrotySlots>().slotTypeInventory;

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
        else if (slotTypeItemMoveParantBefore != SlotType.SlotBoxes)
        {
            Debug.Log("slotTypeItemMoveParantBefore != SlotType.SlotBoxes ");

            if (itemData != null)
            {
                itemData.count += countItemMove;
                itemClassMove.quantityItem -= countItemMove;
                UpdateUIItemMove();
                //    itemData.count += countItemMove;
                //    itemClassInChild.quantityItem -= countItemMove;  
            }
            else
            {
                itemClassMove.quantityItem -= countItemMove;
                if (itemClassMove.quantityItem == 0)
                {
                    itemClassMove.quantityItem = countItemMove;
                    Destroy(itemClassMove.gameObject);
                }

                ItemData newItemData = inventoryItemPresent.ConventItemClassToItemData(itemClassMove);
                inventoryItemPresent.AddItem(newItemData);
            }
        }

        if (itemData != null)
        {
            if (itemData.count <= 0)
            {
                listItemData.Remove(itemData);
            }
        }

        if (itemClassMove.quantityItem <= 0)
        {
            Destroy(itemClassMove.gameObject);
        }

        itemClassInChild = null;
        itemClassMove = null;


        inventoryItemPresent.RefreshUIBox();
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

        gameObject.SetActive(false);
    }
}
