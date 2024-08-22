using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUi : MonoBehaviour
{
    public UpgradeBuilding currentBuildingScript;
    public BuildManager buildManager;
    public UImanger uImanger; 
    public Image image;
    public GameObject Waterimage;
    public GameObject Electiciteisimage;
    public GameObject SpecialistImage;
    public TextMeshProUGUI textNameBuild;
    public TextMeshProUGUI textDescriveBuild;
    public TextMeshProUGUI textPlankCost;
    public TextMeshProUGUI textSteelCost;
    public TextMeshProUGUI textNpcCost;
    public TextMeshProUGUI textDayCost;

    void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>();
        uImanger = FindObjectOfType<UImanger>();
    }

    public void Initialize(UpgradeBuilding upgradeBuilding)
    {
        currentBuildingScript = upgradeBuilding;
        SetDataUpgrade();
    }

    public void SetDataUpgrade()
    {
        textPlankCost.text = currentBuildingScript.plankCost.ToString();
        textSteelCost.text = currentBuildingScript.steelCost.ToString();
        textNpcCost.text = currentBuildingScript.npcCost.ToString();
        textDayCost.text = currentBuildingScript.dayCost.ToString();
        Waterimage.SetActive(currentBuildingScript.isneedwater);
        Electiciteisimage.SetActive(currentBuildingScript.isneedElecticities);
    }

    void OnDisable()
    {
        currentBuildingScript = null;
    }

    // New helper method to check if all required conditions are met
    private bool AreUpgradeConditionsMet()
    {
        // List of conditions to check
        var conditions = new List<(bool condition, string failMessage)>
        {
            // Condition 1: If water is needed, check if it's active
            (!currentBuildingScript.isneedwater || buildManager.iswateractive, 
            "Water is required but not active."),
            
            // Condition 2: If electricity is needed, check if it's active
            (!currentBuildingScript.isneedElecticities || buildManager.iselecticitiesactive, 
            "Electricity is required but not active."),
        };

        // Check all conditions
        foreach (var (condition, failMessage) in conditions)
        {
            if (!condition)
            {
                Debug.Log(failMessage);
                return false;
            }
        }

        return true;
    }

    // New helper method to check if resources are sufficient
    private bool AreResourcesSufficient()
    {
        return buildManager.steel >= currentBuildingScript.steelCost &&
               buildManager.plank >= currentBuildingScript.plankCost &&
               buildManager.npc >= currentBuildingScript.npcCost;
    }

    public void ComfirmUpgrade()
    {
        if (AreUpgradeConditionsMet())
        {
            if (AreResourcesSufficient())
            {
                buildManager.steel -= currentBuildingScript.steelCost;
                buildManager.plank -= currentBuildingScript.plankCost;
                buildManager.npc -= currentBuildingScript.npcCost;
                currentBuildingScript.isBuilding = true;
                uImanger.DisableUpgradeUI();
            }
            else
            {
                Debug.Log("Not enough resources to upgrade.");
            }
        }
    }
}
