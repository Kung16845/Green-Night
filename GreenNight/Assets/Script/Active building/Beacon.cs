using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour
{
     public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeBuilding upgradeBuilding;
    public Globalstat globalstat;
    
    private float currentRiskContribution = 0; // Track the bed contribution for this building
    private int currentOutpostlimit = 0;
    private float currentRewardSpeed = 0;
    private float currentNpcchange = 0;
    private bool isApplied = false;         // Ensure we apply once per stage

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        globalstat = FindObjectOfType<Globalstat>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        upgradeBuilding = GetComponent<UpgradeBuilding>();

        dateTime = timeManager.dateTime;
        currentDay = dateTime.day;
        isApplied = false;
    }

    void Update()
    {
        if (!isApplied && building.isfinsih)
        {
            ApplyRiskContribution(); // Apply initial bed contribution
        }

        if (upgradeBuilding != null && upgradeBuilding.isFinished) 
        {
            UpgradeBeaconContribution(); // Handle upgrade contribution
        }
    }

    void ApplyRiskContribution()
    {
        currentRiskContribution = GetRiskValueBasedOnLevel();
        currentOutpostlimit = GetOutpostValueBasedOnLevel();
        currentRewardSpeed = GetRewardValueBasedOnLevel();
        currentNpcchange = GetNpcchangeValueBasedOnLevel();
        globalstat.DecreaseRiskofExipidition(currentRiskContribution);
        globalstat.IncreaseOutpostlimit(currentOutpostlimit);
        globalstat.IncreaseRewardspeed(currentRewardSpeed);
        globalstat.IncreaseNpcchange(currentNpcchange);
        isApplied = true; // Ensure this runs only once after the building finishes
    }

    void UpgradeBeaconContribution()
    {
        float NewriskContribution = GetRiskValueBasedOnLevel();
        int NewoutpostContribution = GetOutpostValueBasedOnLevel();
        float NewRewardContribution = GetRewardValueBasedOnLevel();
        float NewNpcchangevalue = GetNpcchangeValueBasedOnLevel();
        // Replace the old contribution with the new one
        globalstat.UpdateoOutpostContribution(currentOutpostlimit, NewoutpostContribution);
        globalstat.UpdateRiskofExipiditionContribution(currentRiskContribution, NewriskContribution);
        globalstat.UpdateRewardSpeedContribution(currentRewardSpeed, NewRewardContribution);
        globalstat.UpdateNpcchangeContribution(currentNpcchange, NewNpcchangevalue);
        currentOutpostlimit = NewoutpostContribution;
        currentRiskContribution = NewriskContribution; // Store the new contribution
        currentRewardSpeed = NewRewardContribution;
        currentNpcchange = NewNpcchangevalue;
    }

    float GetRiskValueBasedOnLevel()
    {
        if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {
                case 2:
                    return 15f; // Level 2 contribution
                default:
                    return 15f; // Level 1 contribution
            }
        }
        return 0; // Default to 0 if no upgrade building is found
    }
    float GetRewardValueBasedOnLevel()
    {
        if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {
                case 2:
                    return 25f; // Level 2 contribution
                default:
                    return 0; // Level 1 contribution
            }
        }
        return 0; // Default to 0 if no upgrade building is found
    }
    float GetNpcchangeValueBasedOnLevel()
    {
         if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {
                case 2:
                    return 7f; // Level 2 contribution
                default:
                    return 5; // Level 1 contribution
            }
        }
        return 0; // Default to 0 if no upgrade building is found
    }
    int GetOutpostValueBasedOnLevel()
    {
        switch (upgradeBuilding.currentLevel)
        {
            case 2: return 3; // Level 2
            default: return 1; // Level 1
        }
    }
}
