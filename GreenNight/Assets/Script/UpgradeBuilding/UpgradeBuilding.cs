using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    public string nameBuild;
    public string detailBuild;

    // Upgrade levels
    public List<UpgradeLevel> upgradeLevels = new List<UpgradeLevel>();

    public int currentLevel = 1;
    public int maxLevel;

    public bool isBuilding;
    public bool isFinished;

    public UpgradeUi upgradeUi;
    public TimeManager timeManager;
    public DateTime dateTime;
    public SpriteRenderer spriteRenderer;
    public Sprite ConstructSprite;
    public BuildManager buildManager;
    public Building building;
    public UImanger uImanger;

    public int finishDayBuildingTime;

    void Awake()
    {
        uImanger = FindObjectOfType<UImanger>();
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        dateTime = timeManager.dateTime;
        building = GetComponent<Building>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isBuilding = false;
        isFinished = false;
        maxLevel = upgradeLevels.Count+1;
    }

    void LateUpdate()
    {
        if (isBuilding)
        {
            WaitUpgrade();
        }
    }

    void OnMouseDown()
    {
        if (building.isfinsih && !isBuilding && currentLevel < maxLevel)
        {
            Debug.Log("showupgradeUI");
            uImanger.ActiveUpgradeUI();
            upgradeUi = FindObjectOfType<UpgradeUi>();
            upgradeUi.Initialize(this);
        }
    }

    void WaitUpgrade()
    {
        if (dateTime.day >= finishDayBuildingTime && isBuilding)
        {
            UpgradeLevel completedLevel = upgradeLevels[currentLevel - 1];

            buildManager.npc += completedLevel.npcCost;
            spriteRenderer.sprite = completedLevel.levelSprite;
            isBuilding = false;
            currentLevel++;

            if (currentLevel > maxLevel)
            {
                currentLevel = maxLevel;
                isFinished = true;
            }

            Debug.Log("Upgraded to Level " + currentLevel);
        }
        else if (dateTime.day < finishDayBuildingTime)
        {
            spriteRenderer.sprite = ConstructSprite;
        }
    }
}
[System.Serializable]
public class UpgradeLevel
{
    public int levelNumber;
    public int steelCost;
    public int plankCost;
    public int npcCost;
    public int dayCost;
    public Sprite levelSprite;
    public bool isneedwater;
    public bool isneedElecticities;
    // Add any other properties specific to the upgrade level
}
