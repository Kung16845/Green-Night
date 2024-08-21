using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBuilding : MonoBehaviour
{
    public TimeManager timeManager;
    public DateTime dateTime;
    public BuildManager buildManager;
    public Building building;
    public int foodgainperday;
    public int currentday;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        building = FindObjectOfType<Building>();
        dateTime = timeManager.dateTime;
        currentday = dateTime.day;
    }

    // Update is called once per frame
    void Update()
    {
        foodgain();   
    }
    void foodgain()
    {
        if(dateTime.day != currentday && building.isfinsih)
        {
            buildManager.food += foodgainperday;
            currentday = dateTime.day;   
        }

    }
}
