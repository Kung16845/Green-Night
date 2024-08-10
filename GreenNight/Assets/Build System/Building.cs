using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{   
    public string name;
    public int steelCost;
    public int plankCost;
    public int foodCost;
    public int fuelCost;
    public int ammoCost;
    public int numDayBuindingTime;
    public int finishDayBuildingTime;
    public bool isBuildingLarge;
    public bool isBuildingMedium;
    public bool isBuildingSmall;
    public TimeManager timeManager;
    public DateTime dateTime;
    public SpriteRenderer spriteRenderer;
    private void Start() 
    {
        timeManager = FindObjectOfType<TimeManager>();
        dateTime = timeManager.dateTime;
        finishDayBuildingTime += dateTime.day + numDayBuindingTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update() 
    {
        WaitBuilding();
    }
    public void WaitBuilding()
    {      
        Debug.Log("WaitBuilding");
        
        if(dateTime.day == finishDayBuildingTime)
        {   
            
            spriteRenderer.color = Color.white;
            return;
        }
        else 
        {   
            Debug.Log("Is Building");
            spriteRenderer.color = Color.yellow;
        }
    }
}
