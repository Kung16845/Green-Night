using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Variable System Dragable")]
    public Transform parentAfterDray;
    public Transform parentBeforeDray;
    public Image imageItem;
    [Header("Data Item")]
    public TextMeshProUGUI nameItem;
    public TextMeshProUGUI count;
    public int idItem;
    public void UpdateDataUI(ItemData itemData)
    {
        nameItem.text = itemData.nameItem;
        count.text = itemData.count.ToString() + "/" + itemData.maxCount.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDray = transform.parent;
        parentBeforeDray = parentAfterDray.transform;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        imageItem.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDray);
        imageItem.raycastTarget = true;
    }
}
