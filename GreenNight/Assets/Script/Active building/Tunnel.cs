using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tunnel : MonoBehaviour
{
    public bool tuneelisopen;
    public TimeManager timeManager;
    public DateTime dateTime;
    public int currentDay;
    public UImanger uImanger;
    public BuildManager buildManager;
    public Globalstat globalstat;
    public InventoryItemPresent inventoryItemPresent;
    public List<ItemData> listInvenrotyTogive;
    public SpriteRenderer spriteRenderer;
    private int daycost = 1;
    public bool isclearing;
    private int npcCost = 1;
    public int finishDayBuildingTime = 0;
    public TextMeshProUGUI statusTunel;
    void OnMouseDown()
    {
        uImanger.ToggleUIPanel(UImanger.UIPanel.TunnelUI);
        if(inventoryItemPresent.GetItemCountByID(1020304) >= 5 && !tuneelisopen && !isclearing)
        {
            uImanger.ActivateUIPanel(UImanger.UIPanel.ClearingTunnelUI);
            statusTunel.text = "We have enough dynamite to clear the tunnel.";
        }
        else if (isclearing && !buildManager.iswateractive)
        {
            statusTunel.text = "The tunnel is being cleared, but it was flooded. This might take a bit longer.";
        }
        else if (isclearing && buildManager.iswateractive)
        {
            statusTunel.text = "The tunnel is being cleared,And The water is being drain";
        }
        else if (!isclearing && tuneelisopen && !buildManager.iswateractive)
        {
            statusTunel.text = "The tunnel is open, but it's still flooded with water. We need to activate the pump to drain it.";
        }
        else if (!isclearing && tuneelisopen && buildManager.iswateractive)
        {
            statusTunel.text = "The tunnel is now open! It's a safe route for expeditions without any risk.";
        }
        else if(!tuneelisopen && !isclearing)
        {
            uImanger.DisableUIPanel(UImanger.UIPanel.ClearingTunnelUI);
            statusTunel.text = $"Dynamite collected: {inventoryItemPresent.GetItemCountByID(1020304)}/5";
        }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dateTime = timeManager.dateTime;
    }
    void Update()
    {
        WaitClearingRock();
        CheckMaintainance();
    }
    public void WaitClearingRock()
    {      
        // Debug.Log("WaitBuilding");
        
        if(dateTime.day >= finishDayBuildingTime && isclearing)
        {   
            isclearing = false;
            tuneelisopen = true;
            globalstat.Tunnelaviable = true;
            buildManager.npc += npcCost;
            return;
        }
        else if(dateTime.day < finishDayBuildingTime)
        {   
            return;
        }
    }
    public void InitializeOpenGateway()
    {
        isclearing = true;
        ItemData itemDataToRemove = new ItemData
        {
                idItem = 1020304,
                count = 5
        };
        inventoryItemPresent.RemoveItem(itemDataToRemove);
         dateTime = timeManager.dateTime;
        finishDayBuildingTime += dateTime.day + daycost;
        buildManager.npc -= npcCost;
        uImanger.DisableUIPanel(UImanger.UIPanel.ClearingTunnelUI);
        uImanger.DisableUIPanel(UImanger.UIPanel.TunnelUI);
    }
    void CheckMaintainance()
    {
        if(tuneelisopen && !buildManager.iswateractive)
        {
            spriteRenderer.enabled = false;
            tuneelisopen = false;
            globalstat.Tunnelaviable = false;
        }
        else if(tuneelisopen)
        {
            spriteRenderer.enabled = false;
            tuneelisopen = true;
            globalstat.Tunnelaviable = true;
        }
    }
}
