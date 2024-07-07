using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public DataItems dataItems;
    public InventorySlot inventorySlot;
    public GameObject itemsDrop;
    private void Awake() {
        inventorySlot = FindAnyObjectByType<InventorySlot>();
    }
    // Update is called once per frame
    void Update()
    {
     
        if(dataItems != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                inventorySlot.AddItem(dataItems);
                
                Destroy(itemsDrop);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        DataItems dataItemsTrigger = other.GetComponent<DataItems>();

        if(dataItemsTrigger != null)
        {
            dataItems = dataItemsTrigger;
            itemsDrop = other.gameObject;
        }    
    }
    private void OnTriggerExit2D(Collider2D other) {
        dataItems = null;
        itemsDrop = null;
    }
}
