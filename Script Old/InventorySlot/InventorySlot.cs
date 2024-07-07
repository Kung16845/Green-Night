using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{   
    
    public List<UISlot> listbombSlot;
    public UISlot toolSlot;
    public List<DataItems> listDataItems;
    
    public void AddItem(DataItems dataItems)
    {   
        Debug.Log("Add Items");
        if (dataItems.isResourece)
        {   
             Debug.Log("IsResourece");
            toolSlot.SetDataUI(dataItems);
        }
        else if(!dataItems.isResourece)
        {   
            Debug.Log("IsNotResourece");
            UISlot uISlot = listbombSlot.FirstOrDefault(name => name.iDItems == 0);
            // Debug.Log(uISlot.name);
            // Debug.Log(  "slot nameItems : "+uISlot.nameItems);
            if(uISlot == null) return;
            
            if(uISlot.iDItems == 0)
            {   
                Debug.Log("UIslot is not null");
                uISlot.SetDataUI(dataItems);

            }
        }
    }
    public void RemoveItem(int iditems)
    {   
        Debug.Log("Remove Items");
        if(iditems == 4)
        {
            toolSlot.RemoveDataUI();
        }
        else 
        {
            UISlot uISlot = listbombSlot.FirstOrDefault(id => id.iDItems == iditems);
            uISlot.RemoveDataUI();
        }
        
        
    }
}


