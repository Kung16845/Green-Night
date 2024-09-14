using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InvenrotySlots : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject uIitem = eventData.pointerDrag;
        DraggableItem draggableItem = uIitem.GetComponent<DraggableItem>();
        draggableItem.parentAfterDray = transform;
    }
}
