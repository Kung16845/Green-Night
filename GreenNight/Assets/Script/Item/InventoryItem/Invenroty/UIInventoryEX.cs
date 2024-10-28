using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventoryEX : UIInventory
{
    public float timeScale;
    private void Awake()
    {
        SetValuableUIInventory();
    }
    private void Start()
    {
        // RefreshUIInventory();
    }
    public void SendNpcExpendition()
    {
        CountdownTimeDay countdownTimeDay = this.gameObject.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = timeScale;
        countdownTimeDay.SetStartExpendition();

        npcManager.listNpc.Remove(npcSelecying);
        DateTime dateTime = countdownTimeDay.timeManager.dateTime;
        Debug.Log("Day : "+dateTime.day);
        if(dateTime.day <= countdownTimeDay.finishDayCraftingTime )
        {
            npcManager.listNpcWorkingWIthInOneDay.Add(npcSelecying);
        }
        else 
        {
            npcManager.listNpcWorkingMoreOneDay.Add(npcSelecying);
        }
        
    }
    private void OnEnable()
    {
        RefreshUIInventory();
        
    }
    private void OnDisable()
    {
        ConventDataUIToItemData();
    }


}
