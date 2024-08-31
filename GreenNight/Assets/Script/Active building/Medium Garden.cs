using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumGarden : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public Building building;
    public int foodgainperday;
    public int currentday;
    public UpgradeBuilding upgradeBuilding;
    
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentday = dateTime.day;
    }

    // Update is called once per frame
    void Update()
    {
        foodgain();   
        IsUpgraded();
    }
    void foodgain()
    {
        if(dateTime.day != currentday && building.isfinsih)
        {
            buildManager.food += foodgainperday;
            currentday = dateTime.day;   
        }
    }
    void IsUpgraded()
    {
        if(upgradeBuilding.isfinsih)
        {
            foodgainperday = 5;
        }
    }
}
