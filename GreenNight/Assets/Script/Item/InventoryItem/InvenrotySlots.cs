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
        //ItemData In ListItemData


        if ((slotTypeInventory == SlotType.SlotBag || slotTypeInventory == draggableItem.uITypeItem)
        && transform.childCount == 0)
        {
            draggableItem.parentAfterDray = transform;

            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.draggableItemMove = uIitem.GetComponent<DraggableItem>();


            if (draggableItem.parentBeforeDray.GetComponentInParent<InvenrotySlots>().slotTypeInventory == SlotType.SlotBoxes)
            {
                OpenUIMoveITems(scriptMoveItems);
            }
        }   
        else if (itemClassInChild != null &&uIItemDataDrag.idItem == uIItemDataInChild.idItem && slotTypeInventory != SlotType.SlotBoxes
        && itemClassInChild.quantityItem < itemClassInChild.maxCountItem)
        {
            Debug.Log("UIItemdata In chind Have && slotTypeInventory != SlotType.SlotBoxes && itemClassInChild.quantityItem < itemClassInChild.maxCountItem");
            scriptMoveItems.itemClassMove = itemClassMove;
            scriptMoveItems.itemClassInChild = itemClassInChild;

            OpenUIMoveITems(scriptMoveItems);
        }
        else
        {
            List<ItemData> listItemDataBoxes = inventoryItemPresent.listItemsDataBox;
            ItemData itemData = listItemDataBoxes.FirstOrDefault(item => item.idItem == itemClassMove.idItem);
            
            Debug.Log("Last Conition");
            if (itemData != null)
            {
                itemData.count += itemClassMove.quantityItem;
                
            }
            else
            {
                ItemData newItemData = inventoryItemPresent.ConventItemClassToItemData(itemClassMove);
                
                inventoryItemPresent.AddItem(newItemData);
            }
            Destroy(itemClassMove.gameObject);
        }

        uIItemDataDrag.slotTypeParent = slotTypeInventory;

        inventoryItemPresent.RefreshUIBox();
        inventoryItemPresent.RefreshUIBox();
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