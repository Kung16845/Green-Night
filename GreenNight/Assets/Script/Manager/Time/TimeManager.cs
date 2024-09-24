using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class TimeManager : MonoBehaviour
{
    public SceneSystem sceneSystem1;
    public DateTime dateTime;
    [Header("Tick Setting")]
    [SerializeField] private int tickSeconedIncrease;
    [SerializeField] private int speedGame;
    public int currentTickSeconedIncrease;
    public float timeBetweenTicks = 1;
    public float currentTimeBetweenTricks = 0;
    public static UnityAction<DateTime> OnDateTimeChanged;
    private void Awake()
    {
        dateTime = new DateTime(0, 0, 0, false, sceneSystem1);
        dateTime.SetTimeStartDay();
        currentTickSeconedIncrease = tickSeconedIncrease;
        
    }
    public void AccelerateTime(int speed)
    {   
        speedGame = speed;  
        currentTickSeconedIncrease = tickSeconedIncrease * speed;
    }
    public void SetDayNIght()
    {
        dateTime.isDayNight = true;
    }
    private void Start()
    {
        OnDateTimeChanged?.Invoke(dateTime);
    }
    private void Update()
    {
        currentTimeBetweenTricks += Time.deltaTime;
        if (currentTimeBetweenTricks > timeBetweenTicks)
        {
            currentTimeBetweenTricks = 0;
            Tick();
        }
    }
    public void Tick()
    {
        AdvanceTime();
    }
    public void AdvanceTime()
    {
        dateTime.AdvanceMinutes(currentTickSeconedIncrease);
    }
}
[System.Serializable]
public class DateTime
{
    public int day;
    public int hour;
    public int minutes;
    public bool isDayNight;
    public SceneSystem sceneSystem;
    public DateTime(int day, int hour, int minutes, bool isHaveDayNight, SceneSystem sceneSystem)
    {
        this.day = day;
        this.hour = hour;
        this.minutes = minutes;
        this.isDayNight = isHaveDayNight;
        this.sceneSystem = sceneSystem;
    }
    public void SetTimeStartDay()
    {
        this.hour = 6;
        this.minutes = 0;
    }
    public void SetTimeNightDay()
    {
        this.hour = 21;
        this.minutes = 30;
    }
    public void AdvanceMinutes(int secondToAdvanceBy)
    {
        if (minutes + secondToAdvanceBy >= 60)
        {
            minutes = (minutes + secondToAdvanceBy) % 60;
            hour++;
            AdvanceDay();
        }
        else
        {
            minutes += secondToAdvanceBy;
        }
    }

    public void AdvanceDay()
    {
        if (!isDayNight)
        {
            if (this.hour == 18 && this.minutes == 0)
            {
                SetTimeStartDay();

                this.day++;
            }
        }
        else
        {

            if (this.hour == 24 && this.minutes == 0)
            {
                this.hour = 0;
            }
            else if (this.hour == 4 && this.minutes == 0)
            {

                this.isDayNight = false;
                this.day++;
                SetTimeStartDay();
                sceneSystem.SwitchScene("TownBaseScene");
            }
            else
            {
                SetTimeNightDay();
                sceneSystem.SwitchScene("DefendSceneFare");
            }
        }
    }
}
