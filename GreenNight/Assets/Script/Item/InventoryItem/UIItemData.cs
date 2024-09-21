using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemData : MonoBehaviour
{
    public TextMeshProUGUI count;
    public int idItem;
    public Itemtype itemtype;
    public SlotType slotType;
    public SlotType slotTypeParent;
    public void UpdateDataUI(ItemClass itemClass)
    {
        // Debug.Log(" Run Funtion UpdateDataUI");
        // Debug.Log(itemClass.quantityItem);
        // Debug.Log(slotTypeParent);
        // Debug.Log("Item Class Count / MaxCount " + itemClass.quantityItem + "  " + itemClass.maxCountItem);
        int countItem = itemClass.quantityItem;
        // Debug.Log("Int CoutItem : " + countItem);
        if (slotTypeParent == SlotType.SlotBoxes)
        {
            count.text = countItem.ToString();
        }
        else
        {
            count.text = countItem.ToString() + "/" + itemClass.maxCountItem.ToString();
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
