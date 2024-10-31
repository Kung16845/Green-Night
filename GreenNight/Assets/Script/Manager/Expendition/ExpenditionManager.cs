using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpenditionManager : MonoBehaviour
{   
    public NpcClass npcSelecying;
    public List<ItemData> listItemDataInventoryslot;
    public List<ItemData> listItemDataInventoryEqicment;
    public GameObject uIExOne;
    public GameObject uIExTwo;


    public void OpenUIExpenditionInventoryOne()
    {
        if (uIExOne != null)
        {
            uIExOne.SetActive(true);
        }
    }
    public void OpenUIExpenditionInventoryTwo()
    {
        if (uIExTwo != null)
        {
            uIExTwo.SetActive(true);
        }
    }
}

