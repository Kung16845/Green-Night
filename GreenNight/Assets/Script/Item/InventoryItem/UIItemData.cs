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
    public void UpdateDataUI(ItemData itemData)
    {
        
        count.text = itemData.count.ToString();
       
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
