using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBuilding : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public Building building;
    public int foodGainPerDay;
    public int currentDay;
    public UpgradeBuilding upgradeBuilding;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
    }

    void Update()
    {
        FoodGain();
        UpdateFoodGainPerDay();
    }

    void FoodGain()
    {
        if (dateTime.day != currentDay && building.isfinsih)
        {
            buildManager.food += foodGainPerDay;
            currentDay = dateTime.day;
        }
    }

    void UpdateFoodGainPerDay()
    {
        if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {
                case 1:
                    foodGainPerDay = 2; // Level 1
                    break;
                case 2:
                    foodGainPerDay = 4; // Level 2
                    break;
                default:
                    foodGainPerDay = 2; // Default to Level 1
                    break;
            }
        }
    }
}
