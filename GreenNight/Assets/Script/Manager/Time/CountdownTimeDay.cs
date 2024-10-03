using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CountdownTimeDay : MonoBehaviour
{
    public float timeScale;
    public float ratio;
    public float timeInSeconds;
    public int finishDayCraftingTime;
    public int finishHourCraftingTime;
    public int finishMinutesCraftingTime;
    public TimeManager timeManager;
    
    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        ratio = timeScale / 1000f;
        timeInSeconds = ratio * 60;


        if (timeManager.dateTime.hour + (timeInSeconds / 60) >= 18)
        {
            finishDayCraftingTime = timeManager.dateTime.day + 1;
            finishHourCraftingTime = timeManager.dateTime.hour + (int)(timeInSeconds / 60) - 18;
            finishMinutesCraftingTime = (int)timeInSeconds % 60;
        }
        else
        {
            finishDayCraftingTime = timeManager.dateTime.day;
            finishHourCraftingTime = timeManager.dateTime.hour + (int)(timeInSeconds / 60);
            finishMinutesCraftingTime = (int)timeInSeconds % 60;
        }
        Debug.Log("Day : " + finishDayCraftingTime + " Hour : " + finishHourCraftingTime
        + " Minutes : " + finishMinutesCraftingTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.dateTime.day >= finishDayCraftingTime &&
        timeManager.dateTime.hour >= finishHourCraftingTime &&
        timeManager.dateTime.minutes >= finishMinutesCraftingTime)
        {
           Debug.Log("Success"); 
        }
    }
    
}
