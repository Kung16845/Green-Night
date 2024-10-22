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

    private int bedFactor = 0;

    public void AddBedsFromBuilding(int bedContribution)
    {
        bedFactor += bedContribution;
        UpdateActiveBedCount();
    }

    public void UpdateBuildingBedContribution(int oldContribution, int newContribution)
    {
        // Subtract the old contribution and add the new one
        bedFactor = bedFactor - oldContribution + newContribution;
        UpdateActiveBedCount();
    }

    private void UpdateActiveBedCount()
    {
        activeBed = bedFactor;  // Set activeBed to the total contribution so far
    }
    
    public void DecreaseDiscontent(float discontentContribution)
    {
        Discontent -= discontentContribution;
    }

    public void UpdateDiscontentContribution(float oldContribution, float newContribution)
    {
        Discontent = Discontent + oldContribution - newContribution;
    }
    public void CalculateActionSpeed(float factornumber)
    {
        ActionSpeed = 1 + factornumber; 
    }
}
