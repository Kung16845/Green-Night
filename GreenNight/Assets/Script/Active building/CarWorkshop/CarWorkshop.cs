using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWorkshop : MonoBehaviour
{
    public int availableCar;
    public int UnaviableCar;
    public float FuelRefillRate;
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeUi upgradeUi;
    public UpgradeBuilding upgradeBuilding;
    public UImanger uImanger;
    public Globalstat globalstat;
    private int StandardFuelcost = 5;
    public bool hasUpgradeApplied;
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
        availableCar = 1;
        FuelRefillRate = 1;
    }


    void Update()
    {
        IsElectricActive();
        IsElectricInactive();
    }

    
    void OnMouseDown()
    {
        if (building.isfinsih && !upgradeBuilding.isBuilding)
        {
            uImanger.ToggleUIPanel(UImanger.UIPanel.CarWorkshopUI);
            
            if (upgradeBuilding.currentLevel == upgradeBuilding.maxLevel && !hasUpgradeApplied)
            {
                uImanger.DisableUIPanel(UImanger.UIPanel.CarUpgradeWorkshopUI);
                globalstat.availablecar += 1; // Increment available cars
                StandardFuelcost = 3;

                hasUpgradeApplied = true; // Set the flag to true to prevent reapplying the upgrade logic
            }
            
            AssignUpgradeData();
        }
    }
    private void AssignUpgradeData()
    {
        uImanger.ToggleUIPanel(UImanger.UIPanel.UpgradeUI);
        upgradeUi = FindObjectOfType<UpgradeUi>();
        upgradeUi.Initialize(upgradeBuilding);
        uImanger.ToggleUIPanel(UImanger.UIPanel.UpgradeUI);
    }
    void IsElectricActive()
    {
        if (building.isfinsih && buildManager.iselecticitiesactive)
        {
            
        }
    }

    void IsElectricInactive()
    {
        if (building.isfinsih && !buildManager.iselecticitiesactive)
        {
            
        }
    }
    public void AddCartoGlobalstat()
    {
        globalstat.availablecar += 1;
        buildManager.fuel -= StandardFuelcost;
        globalstat.UnaviableCar -= 1;
    }
    public void DecreaseCartoGlobalstat()
    {
        globalstat.availablecar -= 1;
    }
}