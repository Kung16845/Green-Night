using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterpump : MonoBehaviour
{
    public BuildManager buildManager;
    public Building building;
    public TimeManager timeManager;
    public UpgradeBuilding upgradeBuilding;
    public DateTime dateTime;
    public int currentday;
    public int FuelCost;
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        upgradeBuilding = FindObjectOfType<UpgradeBuilding>();
        building = GetComponent<Building>();
        dateTime = timeManager.dateTime;
        currentday = dateTime.day;
    }
    void Update()
    {
        if(currentday != dateTime.day)
        {
            if(upgradeBuilding.currentLevel != 2)
            {
                if(buildManager.fuel >= FuelCost)
                {
                    buildManager.fuel -= FuelCost;
                    Activewater();
                }
                else if(buildManager.fuel < FuelCost)
                {
                    DeactiveWater();
                }
            }
            else if(upgradeBuilding.currentLevel == 2)
            {
                Activewater();
            }
            currentday = dateTime.day;  
        }
    }
    void Activewater()
    {
        if(building.isfinsih)
        {
            buildManager.iswateractive = true;
        }
    }
     void DeactiveWater()
    {
        buildManager.iswateractive = false;
    }
}
