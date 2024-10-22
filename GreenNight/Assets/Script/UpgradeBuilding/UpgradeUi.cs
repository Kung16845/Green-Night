using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUi : MonoBehaviour
{
    public UpgradeBuilding currentBuildingScript;
    public BuildManager buildManager;
    public DateTime dateTime;
    public TimeManager timeManager;
    public UImanger uImanger;
    public Image image;
    public GameObject WaterImage;
    public GameObject ElectricityImage;
    public TextMeshProUGUI textNameBuild;
    public TextMeshProUGUI textDescribeBuild;
    public TextMeshProUGUI textPlankCost;
    public TextMeshProUGUI textSteelCost;
    public TextMeshProUGUI textNpcCost;
    public TextMeshProUGUI textDayCost;
    public TextMeshProUGUI requiredSpecialistText;
    public NpcManager npcManager;

    void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>();
        timeManager = FindObjectOfType<TimeManager>();
        uImanger = FindObjectOfType<UImanger>();
        npcManager = FindObjectOfType<NpcManager>();
    }

    void Start()
    {
        dateTime = timeManager.dateTime;
    }

    public void Initialize(UpgradeBuilding upgradeBuilding)
    {
        currentBuildingScript = upgradeBuilding;
        SetDataUpgrade();
    }

   public void SetDataUpgrade()
    {
        int nextLevelIndex = currentBuildingScript.currentLevel - 1;

        if (nextLevelIndex >= currentBuildingScript.upgradeLevels.Count)
        {
            Debug.Log("No further upgrades available.");
            return;
        }

        UpgradeLevel nextLevel = currentBuildingScript.upgradeLevels[nextLevelIndex];

        textPlankCost.text = nextLevel.plankCost.ToString();
        textSteelCost.text = nextLevel.steelCost.ToString();
        textNpcCost.text = nextLevel.npcCost.ToString();
        textDayCost.text = nextLevel.dayCost.ToString();
        image.sprite = nextLevel.levelSprite;
        WaterImage.SetActive(nextLevel.isneedwater);
        ElectricityImage.SetActive(nextLevel.isneedElecticities);

        // Update the UI for required specialist
        if (nextLevel.isNeedSpecialist)
        {
            requiredSpecialistText.text = nextLevel.requiredSpecialist.ToString();
            requiredSpecialistText.gameObject.SetActive(true);
        }
        else
        {
            requiredSpecialistText.gameObject.SetActive(false);
        }
    }

     private bool AreUpgradeConditionsMet()
    {
        int nextLevelIndex = currentBuildingScript.currentLevel - 1;
        UpgradeLevel nextLevel = currentBuildingScript.upgradeLevels[nextLevelIndex];

        var conditions = new List<(bool condition, string failMessage)>
        {
            (!nextLevel.isneedwater || buildManager.iswateractive, "Water is required but not active."),
            (!nextLevel.isneedElecticities || buildManager.iselecticitiesactive, "Electricity is required but not active."),
            // Conditionally add the specialist requirement
            (!nextLevel.isNeedSpecialist || HasRequiredSpecialist(nextLevel.requiredSpecialist), $"A {nextLevel.requiredSpecialist} specialist is required but not available."),
        };

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
    private void AssignSpecialistToUpgrade(SpecialistRoleNpc requiredSpecialist)
    {
        // Find the NPC with the required specialist role
        NpcClass specialistNpc = npcManager.listNpc.Find(npc => npc.roleNpc == requiredSpecialist);

        if (specialistNpc != null)
        {
            // Remove the NPC from the available list and add to working list
            npcManager.listNpc.Remove(specialistNpc);
            npcManager.listNpcWorkingMoreOneDay.Add(specialistNpc);

            // Store a reference to the NPC in the building
            currentBuildingScript.assignedSpecialistNpc = specialistNpc;
        }
    }
    private bool HasRequiredSpecialist(SpecialistRoleNpc requiredSpecialist)
    {
        // Check if there is at least one NPC with the required specialist role
        return npcManager.listNpc.Exists(npc => npc.roleNpc == requiredSpecialist);
    }
    private bool AreResourcesSufficient()
    {
        int nextLevelIndex = currentBuildingScript.currentLevel - 1;
        UpgradeLevel nextLevel = currentBuildingScript.upgradeLevels[nextLevelIndex];

        return buildManager.steel >= nextLevel.steelCost &&
               buildManager.plank >= nextLevel.plankCost &&
               buildManager.npc >= nextLevel.npcCost;
    }

    public void ConfirmUpgrade()
    {
        if (AreUpgradeConditionsMet())
        {
            if (AreResourcesSufficient())
            {
                int nextLevelIndex = currentBuildingScript.currentLevel - 1;
                UpgradeLevel nextLevel = currentBuildingScript.upgradeLevels[nextLevelIndex];

                // Subtract resources
                buildManager.steel -= nextLevel.steelCost;
                buildManager.plank -= nextLevel.plankCost;
                buildManager.npc -= nextLevel.npcCost;

                // Conditionally assign the specialist NPC to the upgrade task
                if (nextLevel.isNeedSpecialist)
                {
                    AssignSpecialistToUpgrade(nextLevel.requiredSpecialist);
                }

                currentBuildingScript.isBuilding = true;
                currentBuildingScript.finishDayBuildingTime = dateTime.day + nextLevel.dayCost;

                uImanger.DisableUpgradeUI();
            }
            else
            {
                Debug.Log("Not enough resources to upgrade.");
            }
        }
    }

}
