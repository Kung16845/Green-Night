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

        if (itemClassInChild == null)
        {
            if (countItemMove >= itemClassMove.quantityItem)
            {
                countItemMove = itemClassMove.quantityItem;
            }

            
        }
        else
        {
            Debug.Log("item classInChild is not null ");
            if (itemClassInChild.quantityItem + countItemMove > itemClassMove.maxCountItem
            && countItemMove > itemClassMove.quantityItem)
            {
                countItemMove = itemClassMove.quantityItem;
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
        gameObject.SetActive(false);

        List<ItemData> listItemData = inventoryItemPresent.listItemsDataBox;
        ItemData itemData = listItemData.FirstOrDefault(item => item.idItem == itemClassMove.idItem);

        itemData.count -= countItemMove;
        itemClassMove.quantityItem = countItemMove;

        if (draggableItemMove.parentBeforeDray.GetComponent<InvenrotySlots>().slotTypeInventory == SlotType.SlotBoxes
        && itemClassInChild == null)
        {
            UpdateUIItemMove();

        }
        else if (itemClassInChild != null)
        {

            // itemData.count -= countItemMove;
            // itemClassMove.quantityItem -= countItemMove;

            itemClassInChild.quantityItem += countItemMove;


            GameObject uIItemInChildObject = itemClassInChild.gameObject;
            UIItemData uIItemDataInChild = uIItemInChildObject.GetComponent<UIItemData>();
            uIItemDataInChild.UpdateDataUI(itemClassInChild);

            if(itemClassMove.quantityItem > 0)
                UpdateUIItemMove();
                
            Debug.Log("Not Parant Slot is SlotBoxes");
        }

        if(itemData.count <= 0)
        {
            listItemData.Remove(itemData);
            if (itemClassMove != null)
            {
                Destroy(itemClassMove.gameObject);
            }

        }
        
        itemClassInChild = null;
        inventoryItemPresent.RefreshUIBox();
    }
    public void UpdateUIItemMove()
    {
        GameObject uIItemObject = itemClassMove.gameObject;
        UIItemData uIItemData = uIItemObject.GetComponent<UIItemData>();
        uIItemData.UpdateDataUI(itemClassMove);
    }
    public void CancleMove()
    {
        gameObject.SetActive(false);
        draggableItemMove.gameObject.transform.SetParent(draggableItemMove.parentBeforeDray);
        draggableItemMove.parentAfterDray = draggableItemMove.parentBeforeDray;
    }
}
