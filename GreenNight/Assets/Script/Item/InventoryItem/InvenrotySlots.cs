using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class InvenrotySlots : MonoBehaviour, IDropHandler
{
    public SlotType slotTypeInventory;
    public GameObject uIMoveItemsBoxesToInventory;
    public Canvas canvas;
    public int maxCountItems;
    public InventoryItemPresent inventoryItemPresent;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

        ScriptMoveItems[] objectsWithScript = Resources.FindObjectsOfTypeAll<ScriptMoveItems>();
        foreach (var item in objectsWithScript)
        {
            GameObject obj = item.gameObject;
            if (!obj.activeInHierarchy)
            {
                uIMoveItemsBoxesToInventory = obj;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // GameObject Dragging
        GameObject uIitem = eventData.pointerDrag;
        DraggableItem draggableItem = uIitem.GetComponent<DraggableItem>();
        UIItemData uIItemDataDrag = uIitem.GetComponent<UIItemData>();
        ItemClass itemClassMove = uIitem.GetComponent<ItemClass>();
        // GameObject InChild if Have
        UIItemData uIItemDataInChild = GetComponentInChildren<UIItemData>();
        ItemClass itemClassInChild = GetComponentInChildren<ItemClass>();
        // Script Move
        ScriptMoveItems scriptMoveItems = uIMoveItemsBoxesToInventory.GetComponent<ScriptMoveItems>();

        if ((slotTypeInventory == SlotType.SlotBag || slotTypeInventory == draggableItem.uITypeItem)
        && transform.childCount == 0)
        {
            draggableItem.parentAfterDray = transform;

            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();

            // scriptMoveItems.countItemMove = 1;
            // scriptMoveItems.countText.text = "1";
            if (draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>().slotTypeInventory == SlotType.SlotBoxes)
            {
                OpenUIMoveITems(scriptMoveItems);
            }
        }
        else if (slotTypeInventory == SlotType.SlotBoxes && uIItemDataInChild == null)
        {
            ItemData itemDataMove = inventoryItemPresent.listItemsDataBox.FirstOrDefault(item => item.idItem == uIItemDataDrag.idItem);
            Debug.Log("slotTypeInventory == SlotType.SlotBoxes");
            scriptMoveItems.itemClassMove = itemClassMove;
            if (itemDataMove != null)
            {
                Debug.Log("ItemDataMove Not null");
     
                OpenUIMoveITems(scriptMoveItems);
            }
            else 
            {   

                OpenUIMoveITems(scriptMoveItems);
            }
        }
        else if (uIItemDataDrag.idItem == uIItemDataInChild.idItem && slotTypeInventory != SlotType.SlotBoxes 
        && itemClassInChild.quantityItem < itemClassInChild.maxCountItem)
        {
            Debug.Log("UIItemdata In chind Have");
            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.itemClassInChild = itemClassInChild;
            // scriptMoveItems.countItemMove = 1;
            // scriptMoveItems.countText.text = "1";
            OpenUIMoveITems(scriptMoveItems);

        }
        else 
        {
            Debug.Log("Last Conition");
        }
        uIItemDataDrag.slotTypeParent = slotTypeInventory;

    }

    public void OpenUIMoveITems(ScriptMoveItems scriptMoveItems)
    {

        scriptMoveItems.countItemMove = 1;
        scriptMoveItems.countText.text = "1";
        uIMoveItemsBoxesToInventory.SetActive(true);

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera, // กล้องที่ใช้ใน Canvas (ถ้าเป็น World Space)
            out mousePos);

        uIMoveItemsBoxesToInventory.GetComponent<RectTransform>().anchoredPosition = mousePos;

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