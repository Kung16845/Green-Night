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
        // Debug.Log("Item Class");
        // Debug.Log(itemClass.quantityItem);
        // Debug.Log(slotTypeParent);
        
        if(slotTypeParent == SlotType.SlotBoxes)
            count.text = itemClass.quantityItem.ToString();
        else 
            count.text = itemClass.quantityItem.ToString() + "/" + itemClass.maxCountItem.ToString();
       
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
