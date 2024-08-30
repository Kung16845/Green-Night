using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItemPresent : MonoBehaviour
{   
    public List<ItemData> listItemsData = new List<ItemData>();
    public List<ItemClass> listUIItem; 
    public Transform inventoryUI;
    private void Start() {
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
            Instantiate(uiItem,inventoryUI,false);
        }

    }
}
