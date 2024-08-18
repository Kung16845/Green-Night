using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{   
    public string nameBuild;
    public string detailBuild;
    public int steelCost;
    public int plankCost;
    public int foodCost;
    public int fuelCost;
    public int ammoCost;
    public int npcCost;
    public int numDayBuindingTime;
    public int finishDayBuildingTime;
    public bool isBuildingLarge;
    public bool isBuildingMedium;
    public bool isBuildingSmall;
    public bool isBuilding;
    public TimeManager timeManager;
    public DateTime dateTime;
    public SpriteRenderer spriteRenderer;
    public BuildManager buildManager;
    private void Start() 
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        dateTime = timeManager.dateTime;
        finishDayBuildingTime += dateTime.day + numDayBuindingTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isBuilding = true;
    }
    private void Update() 
    {
        WaitBuilding();
    }
    public void WaitBuilding()
    {      
        Debug.Log("WaitBuilding");
        
        if(dateTime.day >= finishDayBuildingTime && isBuilding)
        {   
            isBuilding = false;
            buildManager.npc += npcCost;
            spriteRenderer.color = Color.white;
            return;
        }
        else if(dateTime.day < finishDayBuildingTime)
        {   
            Debug.Log("Is Building");
            spriteRenderer.color = Color.yellow;
        }
    }
}
