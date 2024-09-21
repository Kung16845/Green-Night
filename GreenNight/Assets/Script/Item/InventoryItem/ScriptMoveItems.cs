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
    public DraggableItem draggableItemMove;
    public InventoryItemPresent inventoryItemPresent;
    // Start is called before the first frame update
    void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
    }
    public void IncreateCountItem(int count)
    {
        countItemMove += count;

        if (countItemMove >= itemClassMove.quantityItem)
        {
            countItemMove = itemClassMove.quantityItem;
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
        List<ItemData> listItemData = inventoryItemPresent.listItemsData;
        ItemData itemData = listItemData.FirstOrDefault(item => item.idItem == itemClassMove.idItem);
        itemData.count -= countItemMove;
        itemClassMove.quantityItem = countItemMove;

        if (itemData.count != 0)
        {   
            Debug.Log("RefreshUIBoxes");
            inventoryItemPresent.RefreshUIBox();
        }
        else 
        {
            listItemData.Remove(itemData);
        }

        GameObject uIItemObject = itemClassMove.gameObject;
        UIItemData uIItemData = uIItemObject.GetComponent<UIItemData>();
        uIItemData.UpdateDataUI(itemClassMove);
        inventoryItemPresent.RefreshUIBox();
    }
    public void CancleMove()
    {
        gameObject.SetActive(false);
        draggableItemMove.gameObject.transform.SetParent(draggableItemMove.parentBeforeDray);
    }
}
