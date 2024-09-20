using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class InvenrotySlots : MonoBehaviour, IDropHandler
{
    public SlotType slotTypeInventory;
    public GameObject uIMoveItems;
    public int maxCountItems;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject uIitem = eventData.pointerDrag;
        DraggableItem draggableItem = uIitem.GetComponent<DraggableItem>();
        if (slotTypeInventory == SlotType.SlotBag || slotTypeInventory == draggableItem.uITypeItem )
        {
            draggableItem.parentAfterDray = transform;  
            uIMoveItems.SetActive(true);
            uIMoveItems.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }
    }
}
public enum SlotType
{
    SlotWeapon,
    SlotVest,
    SlotTool,
    SlotBackpack,
    SlotBag,
    SlotLock,
    SlotBoxes
}