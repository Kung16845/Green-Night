using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    public float Actionspeedincrease;
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeBuilding upgradeBuilding;
    public Globalstat globalstat;
    public bool Isapplyspeed;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        globalstat = FindObjectOfType<Globalstat>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
        Isapplyspeed = false;
    }
    void Update()
    {
        IsElecticActive();
    }
    void IsElecticActive()
    {
        if(building.isfinsih && buildManager.iselecticitiesactive)
        {
            float IncreaseActionSpeed = 0.25f;
            if(!Isapplyspeed)
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
            globalstat.CalculateActionSpeed(-DecreaseActionSpeed);
        }
    }
}
