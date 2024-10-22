using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lounge : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeBuilding upgradeBuilding;
    public Globalstat globalstat;
    private float Discontent;
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
            globalstat.DecreaseDiscontent(Discontent);
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
                    IncreaseBed = 3; // Level 2
                    Discontent = 25f;
                    isapply = false;
                    break;
                case 3:
                    IncreaseBed = 5; // Level 3
                    Discontent = 35f;
                    isapply = false;
                    break;
                default:
                    IncreaseBed = 1; // Level 1
                    Discontent = 15f;
                    break;
            }
        }
    }
}
