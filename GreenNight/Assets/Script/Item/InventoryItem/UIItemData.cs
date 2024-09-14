using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemData : MonoBehaviour 
{
    
    
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
    
}
