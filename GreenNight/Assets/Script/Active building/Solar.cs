using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : MonoBehaviour
{
    public BuildManager buildManager;
    public Building building;
    public TimeManager timeManager;
    public UpgradeBuilding upgradeBuilding;
    public DateTime dateTime;
    public int currentday;
    public int steelCost;
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
            if(!upgradeBuilding.isfinsih)
            {
                if(buildManager.fuel >= steelCost)
                {
                    buildManager.fuel -= steelCost;
                    ActiveElecticities();
                }
                else if(buildManager.fuel < steelCost)
                {
                    DeactiveElecticities();
                }
            }
            else if(upgradeBuilding.isfinsih)
            {
                ActiveElecticities();
            }
            currentday = dateTime.day;  
        }
    }
    void ActiveElecticities()
    {
        if(building.isfinsih)
        {
            buildManager.iselecticitiesactive = true;
        }
    }
    void DeactiveElecticities()
    {
        buildManager.iselecticitiesactive = false;
    }
}
