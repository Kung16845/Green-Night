using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globalstat : MonoBehaviour
{
    [Header("DDAPoint")]
    public float CommunitysupplyPoint;
    public float Skillplaypoint;
    public float itemponts;
    public int ProgressDay;
    public float succesfulDefense;
    [Header("Action Speed")]
    public float ActionSpeed;
    public float Healingspeed;
    public float Craftingspeed;
    public float BuildingSpeed;
    [Header("DamageFactor")]
    public float WeapondDamge;
    public float Damageincrease;
    public float Carendurance;
    public float fuelefficiency;
    [Header("Expidition")]
    public float Radiospeed;
    public float Outpostspeed;
    public float expiditionspeed;
    public float expiditionrisk;
    [Header("Community Stat")]
    public float illResistance;
    public float Npcchange;
    public int activeBed;
    public int activeCureBed;
    public float Discontent;
    public float Growingspeed;
    public float OutpostLimit;

    private int bedFactor = 1;

    public void AddBedsFromBuilding(int bedContribution)
    {
        bedFactor += bedContribution;  
        UpdateActiveBedCount();        
    }
    private void UpdateActiveBedCount()
    {
        activeBed = bedFactor;  // Set activeBed to the total contribution so far.
        Debug.Log("Total Beds: " + activeBed);
    }
    public void DecreaseDiscontent(float Discontentfactor)
    {
        Discontent = 0 - Discontentfactor;
    }
    public void CalculateActionSpeed(float factornumber)
    {
        ActionSpeed = 1 + factornumber; 
    }
}
