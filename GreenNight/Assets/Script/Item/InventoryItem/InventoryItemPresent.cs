using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryItemPresent : MonoBehaviour
{
    public List<ItemData> listItemsDataBox = new List<ItemData>();
    public List<UIItemData> listUIItemPrefab;
    public List<InvenrotySlots> listInvenrotySlots = new List<InvenrotySlots>();
    public InvenrotySlots invenrotySlotSpecialMilitaryLock;
    public InvenrotySlots invenrotySlotSpecialScavenger;
    public Transform transformsBoxes;

    private void Start()
    {
        RefreshUIBox();
        RefreshUIBox();
    }

    public void RefreshUIBox()
    {
        ClearUIBoxes();
        
        foreach (ItemData itemData in listItemsDataBox)
        {
            CreateUIBoxes(itemData);
        }

    }
    public void RefreshUIBoxCategory(int numCategory)
    {   
        ClearUIBoxes();
        Itemtype itemtypeCategory = (Itemtype)numCategory;

        foreach (ItemData itemData in listItemsDataBox)
        {
            if (itemData.itemtype == itemtypeCategory)
            {
                CreateUIBoxes(itemData);
                
            }
        }

    }
    public void CreateUIBoxes(ItemData itemData)
    {   

        GameObject uiItem = listUIItemPrefab.FirstOrDefault(idItem => idItem.idItem == itemData.idItem).gameObject;
        GameObject uIItemOBJ = Instantiate(uiItem, transformsBoxes, false);

        UIItemData uIItemData = uIItemOBJ.GetComponent<UIItemData>();
        ItemClass itemClass = uIItemOBJ.GetComponent<ItemClass>();

        itemClass.quantityItem = itemData.count;
        itemClass.maxCountItem = itemData.maxCount;

        uIItemData.slotTypeParent = transformsBoxes.GetComponent<InvenrotySlots>().slotTypeInventory;
        uIItemData.UpdateDataUI(itemClass);
    }
    public void ClearUIBoxes()
    {
        foreach (Transform child in transformsBoxes)
        {
            Destroy(child.gameObject);
        }

    }

    public void AddItem(ItemData itemDataAdd)
    {

        ItemData itemDataInList = listItemsDataBox.FirstOrDefault(item => item.idItem == itemDataAdd.idItem && item.count != item.maxCount);

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

                listItemsDataBox.Add(newItemData);

                itemDataInList.count = itemDataInList.maxCount;

            }
        }
        else
        {
            listItemsDataBox.Add(itemDataAdd);
        }

        RefreshUIBox();
    }
    public void RemoveItem(ItemData itemDataRemove)
    {

        ItemData itemDataInList = listItemsDataBox.LastOrDefault(item => item.idItem == itemDataRemove.idItem);

        if (itemDataInList.count - itemDataRemove.count >= 0)
        {
            itemDataInList.count -= itemDataRemove.count;
            if (itemDataInList.count == 0)
            {
                listItemsDataBox.Remove(itemDataInList);
            }
        }
        else
        {
            //ถ้าไปเท็มในกล่องไม่พอให้ทำอะไร
        }
        RefreshUIBox();
    }
    public ItemData ConventItemClassToItemData(ItemClass itemClass)
    {
        ItemData newItemData = new ItemData();

        newItemData.nameItem = itemClass.nameItem;
        newItemData.idItem = itemClass.idItem;
        newItemData.count = itemClass.quantityItem;
        newItemData.maxCount = itemClass.maxCountItem;
        newItemData.itemtype = itemClass.itemtype;

        return newItemData;
    }
}
