using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemPresent : MonoBehaviour
{
    public List<ItemData> listItemsData = new List<ItemData>();
    public List<UIItemData> listUIItem;
    public Transform inventoryUI;
    private void Start()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        foreach (Transform child in inventoryUI)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData itemData in listItemsData)
        {
            GameObject uiItem = listUIItem.FirstOrDefault(idItem => idItem.idItem == itemData.idItem).gameObject;
            Instantiate(uiItem, inventoryUI, false);

            UIItemData uIItemData = uiItem.GetComponent<UIItemData>();
            uIItemData.UpdateDataUI(itemData);
        }

    }
    public void AddItem(ItemData itemDataAdd)
    {

        ItemData itemDataInList = listItemsData.FirstOrDefault(item => item.idItem == itemDataAdd.idItem && item.count != item.maxCount);

        if (itemDataInList != null)
        {   
            int itemCount = itemDataInList.count + itemDataAdd.count;
            if (itemCount <= itemDataInList.maxCount)
            {
                itemDataInList.count += itemDataAdd.count;
            }
            else if (itemCount >= itemDataInList.maxCount)
            {
                Debug.Log("ITem new create count : " + itemDataAdd.count);

                ItemData newItemData = new ItemData();
                newItemData.nameItem = itemDataInList.nameItem;
                newItemData.idItem = itemDataInList.idItem;
                newItemData.count = itemCount - itemDataInList.maxCount;
                newItemData.maxCount = itemDataInList.maxCount;
                newItemData.itemtype = itemDataInList.itemtype;
            
                listItemsData.Add(newItemData);

                itemDataInList.count = itemDataInList.maxCount;

                
            }
        }
        else
        {
            listItemsData.Add(itemDataAdd);
        }

        RefreshUI();
    }
    public void RemoveItem(ItemData itemDataRemove)
    {

        ItemData itemDataInList = listItemsData.FirstOrDefault(item => item.idItem == itemDataRemove.idItem);
        itemDataInList.count--;
        if (itemDataInList.count <= 0)
        {
            listItemsData.Remove(itemDataInList);
        }

    }
}
