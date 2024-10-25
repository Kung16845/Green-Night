using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBed : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public int currentDay;
    public Building building;
    public UpgradeBuilding upgradeBuilding;
    public Globalstat globalstat;
    
    private int currentBedContribution = 0; // Track the bed contribution for this building
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
            ApplyBedContribution(); // Apply initial bed contribution
        }

        if (upgradeBuilding != null && upgradeBuilding.isFinished) 
        {
            UpgradeBedContribution(); // Handle upgrade contribution
        }
    }

    void ApplyBedContribution()
    {
        currentBedContribution = GetBedValueBasedOnLevel();
        globalstat.AddBedsFromBuilding(currentBedContribution);
        isApplied = true; // Ensure this runs only once after the building finishes
    }

    void UpgradeBedContribution()
    {
        int newBedContribution = GetBedValueBasedOnLevel();

        // Replace the old contribution with the new one
        globalstat.UpdateBuildingBedContribution(currentBedContribution, newBedContribution);

        currentBedContribution = newBedContribution; // Store the new contribution
    }

    int GetBedValueBasedOnLevel()
    {
        if (upgradeBuilding != null)
        {
            switch (upgradeBuilding.currentLevel)
            {
                case 2:
                    return 4; // Level 2 contribution
                default:
                    return 2; // Level 1 contribution
            }
        }
        return 0; // Default to 0 if no upgrade building is found
    }
}

