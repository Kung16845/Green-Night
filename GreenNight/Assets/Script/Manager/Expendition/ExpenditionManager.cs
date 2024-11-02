using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

using UnityEngine.UI;

public class ExpenditionManager : MonoBehaviour
{
    public NpcClass npcSelecying;
    public List<ItemData> listItemDataInventoryslot;
    public List<ItemData> listItemDataInventoryEqicment;
    public GameObject uIExOne;
    public GameObject uIExTwo;
    public GameObject playerObject;
    public UIButtonEX uIButtonEXOne;
    public UIButtonEX uIButtonEXTwo;
    public Globalstat globalstat;
    public InventoryItemPresent inventoryItemPresent;
    private void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        globalstat = FindObjectOfType<Globalstat>();
    }
    public void SetUIExButton(int indexEXUI, Sprite spriteHeadNpc, string textdayFinish)
    {
        if (indexEXUI == 1)
        {
            // uIEx
            uIButtonEXOne.GetComponent<UIButtonEX>().SetUIButtonEX(spriteHeadNpc, textdayFinish);
        }
        else
        {
            uIButtonEXTwo.GetComponent<UIButtonEX>().SetUIButtonEX(spriteHeadNpc, textdayFinish);
        }


    }

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
    public bool IsActiveEvent()
    {
        float randomValue = Random.Range(0f, 100f);
        return randomValue <= globalstat.expiditionrisk;
    }
}

