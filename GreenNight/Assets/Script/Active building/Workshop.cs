using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : MonoBehaviour
{
    public float Actionspeedincrease;
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeUi upgradeUi;
    public UpgradeBuilding upgradeBuilding;
    public UImanger uImanger;
    public Globalstat globalstat;
    public int Craftingslot;
    public bool Isapplyspeed;
    public List<CraftingItem> craftingItemsLevel1;
    public List<CraftingItem> craftingItemsLevel2;

    void Start()
    {
        uImanger = FindObjectOfType<UImanger>();
        timeManager = FindObjectOfType<TimeManager>();
        globalstat = FindObjectOfType<Globalstat>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
        Actionspeedincrease = 0.25f;
        Craftingslot = 3;
        Isapplyspeed = false;
    }

    void Update()
    {
        IsElectricActive();
        IsElectricInactive();
    }

    void OnMouseDown()
    {
        if (building.isfinsih && !upgradeBuilding.isBuilding && upgradeBuilding.currentLevel < upgradeBuilding.maxLevel)
        {
            uImanger.ActiveWorkshopUI();
            AssignUpgradeData();
        }
    }
    private void AssignUpgradeData()
    {
        uImanger.ActiveUpgradeUI();
        upgradeUi = FindObjectOfType<UpgradeUi>();
        upgradeUi.Initialize(upgradeBuilding);
        uImanger.DisableUpgradeUI();
    }
    void IsElectricActive()
    {
        if (building.isfinsih && buildManager.iselecticitiesactive)
        {
            float IncreaseActionSpeed = 0.25f;
            if (!Isapplyspeed)
            {
                globalstat.CalculateActionSpeed(IncreaseActionSpeed);
                Isapplyspeed = true;
            }
        }
    }

    void IsElectricInactive()
    {
        if (building.isfinsih && !buildManager.iselecticitiesactive)
        {
            float DecreaseActionSpeed = 0.25f;
            if (Isapplyspeed)
            {
                globalstat.CalculateActionSpeed(-DecreaseActionSpeed);
                Isapplyspeed = false;
            }
        }
    }
}
