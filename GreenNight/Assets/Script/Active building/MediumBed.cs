using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumBed : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeBuilding upgradeBuilding;
    public Globalstat globalstat;
    private int IncreaseBed;
    private bool isapply;
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        globalstat = FindObjectOfType<Globalstat>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();
        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
        isapply = false;
    }
    void Update()
    {
        applyabilities();
        IsUpgraded();
    }
    void applyabilities()
    {
        if(!isapply && building.isfinsih)
        {
            globalstat.AddBedsFromBuilding(IncreaseBed);
            isapply = true; 
        }
    }
    void IsUpgraded()
    {
       if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {

                case 2:
                    IncreaseBed = 6; // Level 2
                    isapply = false;
                    break;
                default:
                    IncreaseBed = 5; // Level 1
                    break;
            }
        }
    }
}
