using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class InvenrotySlots : MonoBehaviour, IDropHandler
{
    public SlotType slotTypeInventory;
    public GameObject uIMoveItems;
    public Canvas canvas;
    public int maxCountItems;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject uIitem = eventData.pointerDrag;
        DraggableItem draggableItem = uIitem.GetComponent<DraggableItem>();
        UIItemData uIItemData = uIitem.GetComponent<UIItemData>();
        if (slotTypeInventory == SlotType.SlotBag || slotTypeInventory == draggableItem.uITypeItem)
        {
            draggableItem.parentAfterDray = transform;
            uIMoveItems.SetActive(true);

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera, // กล้องที่ใช้ใน Canvas (ถ้าเป็น World Space)
                out mousePos);

            uIMoveItems.GetComponent<RectTransform>().anchoredPosition = mousePos;
            uIMoveItems.GetComponent<ScriptMoveItems>().itemClassMove = uIitem.GetComponent<ItemClass>();
            uIMoveItems.GetComponent<ScriptMoveItems>().draggableItemMove = uIitem.GetComponent<DraggableItem>();
            
        }
        uIItemData.slotTypeParent = slotTypeInventory;
        
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