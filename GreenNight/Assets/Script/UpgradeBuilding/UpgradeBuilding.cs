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
    public NpcClass assignedSpecialistNpc;
    public BuiltBuildingInfo builtBuildingInfo;

    public int finishDayBuildingTime;

    void Awake()
    {
        uImanger = FindObjectOfType<UImanger>();
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        dateTime = timeManager.dateTime;
        building = GetComponent<Building>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildManager = FindObjectOfType<BuildManager>();
        isBuilding = false;
        isFinished = false;
        maxLevel = upgradeLevels.Count+1;
    }
    void Start()
    {
        foreach (var builtBuilding in buildManager.builtBuildings)
        {
            if (builtBuilding.buildingGameObject == this.gameObject)
            {
                builtBuildingInfo = builtBuilding;
                break;
            }
        }
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

            // Update the level in builtBuildingInfo
            if (builtBuildingInfo != null)
            {
                builtBuildingInfo.level = currentLevel;
            }

            if (currentLevel == maxLevel)
            {
                isFinished = true;
            }

            // Return the assigned specialist NPC to the available list
            if (assignedSpecialistNpc != null)
            {
                NpcManager npcManager = FindObjectOfType<NpcManager>();
                npcManager.listNpcWorkingMoreOneDay.Remove(assignedSpecialistNpc);
                npcManager.listNpc.Add(assignedSpecialistNpc);
                assignedSpecialistNpc = null;
            }

            Debug.Log("Upgraded to Level " + currentLevel);
        }
        else if (dateTime.day < finishDayBuildingTime)
        {
            spriteRenderer.sprite = ConstructSprite;
        }
    }
    // public void DisableCollider()
    // {
    //     Collider2D collider = GetComponent<Collider2D>();
    //     if (collider != null)
    //     {
    //         collider.enabled = false;
    //     }
    // }

    // public void EnableCollider()
    // {
    //     Collider2D collider = GetComponent<Collider2D>();
    //     if (collider != null)
    //     {
    //         collider.enabled = true;
    //     }
    // }
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

    // Add this line to include the required specialist role
    public bool isNeedSpecialist;
    public SpecialistRoleNpc requiredSpecialist;
}
