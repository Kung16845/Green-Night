using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBuilding : MonoBehaviour
{
    public string nameBuild;
    public string detailBuild;
    public int steelCost;
    public int plankCost;
    public bool isneedwater;
    public bool isneedElecticities;
    // public bool isneedspeciallist;
    public int npcCost;
    public int dayCost;
    public int finishDayBuildingTime;
    public bool isBuilding;
    public UpgradeUi upgradeUi;
    public TimeManager timeManager;
    public DateTime dateTime;
    public SpriteRenderer spriteRenderer;
    public BuildManager buildManager;
    public Building building;
    public UImanger uImanger; // Change to GameObject
    public bool isfinsih;

    void Awake()
    {
        uImanger = FindObjectOfType<UImanger>();
        // UpgradeUI = FindObjectOfType<UpgradeUi>().gameObject;
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        dateTime = timeManager.dateTime;
        building = GetComponent<Building>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isBuilding = false;
        isfinsih = false;
    }
    void LateUpdate()
    {
        if(isBuilding)
        {
            WaitUpgrade();
        }
    }

    void OnMouseDown()
    {
        if(building.isfinsih && !isfinsih && !isBuilding)
        {
            uImanger.ActiveUpgradeUI();
            upgradeUi = FindObjectOfType<UpgradeUi>();
            upgradeUi.Initialize(this);
        }
    }
    void WaitUpgrade()
    {      
        if(dateTime.day >= finishDayBuildingTime && isBuilding)
        {   
            buildManager.npc += npcCost;
            spriteRenderer.color = Color.white;
            isfinsih = true;
            isBuilding = false;
            Debug.Log("Upgraded");
            return;
        }
        else if(dateTime.day < finishDayBuildingTime)
        {   
            spriteRenderer.color = Color.yellow;
        }
    }
}
