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

    void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>();
        timeManager = FindObjectOfType<TimeManager>();
        uImanger = FindObjectOfType<UImanger>();
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
    }

    void OnDisable()
    {
        currentBuildingScript = null;
    }

    private bool AreUpgradeConditionsMet()
    {
        int nextLevelIndex = currentBuildingScript.currentLevel - 1;
        UpgradeLevel nextLevel = currentBuildingScript.upgradeLevels[nextLevelIndex];

        var conditions = new List<(bool condition, string failMessage)>
        {
            (!nextLevel.isneedwater || buildManager.iswateractive, "Water is required but not active."),
            (!nextLevel.isneedElecticities || buildManager.iselecticitiesactive, "Electricity is required but not active."),
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

                buildManager.steel -= nextLevel.steelCost;
                buildManager.plank -= nextLevel.plankCost;
                buildManager.npc -= nextLevel.npcCost;

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
