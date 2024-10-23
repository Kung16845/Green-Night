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
    public float RadioCooldownspeed;
    public float Outpostrewardspeed;
    public float expiditionspeed;
    public float expiditionrisk;
    public float OutpostLimit;
    [Header("Community Stat")]
    public float illResistance;
    public float Npcchange;
    public int activeBed;
    public int activeCureBed;
    public float Discontent;
    public float Growingspeed;

    private int bedFactor = 0;
    //ActiveBed
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
    //Discontent
    public void DecreaseDiscontent(float discontentContribution)
    {
        Discontent -= discontentContribution;
    }
    public void UpdateDiscontentContribution(float oldContribution, float newContribution)
    {
        Discontent = Discontent + oldContribution - newContribution;
    }
    //OutpostLimit
    public void IncreaseOutpostlimit(float Outpostcontribution)
    {
        OutpostLimit += Outpostcontribution;
    }
    public void UpdateoOutpostContribution(float oldContribution, float newContribution)
    {
        OutpostLimit = OutpostLimit + newContribution - oldContribution;
    }
    //Riskofexpidition
    public void DecreaseRiskofExipidition(float RiskContribution)
    {
        expiditionrisk -= RiskContribution;
    }
    public void UpdateRiskofExipiditionContribution(float oldContribution, float newContribution)
    {
        expiditionrisk = expiditionrisk + oldContribution - newContribution;
    }
    //IncreaseReward speed
    public void IncreaseRewardspeed(float RewardSpeedContribution)
    {
       Outpostrewardspeed -= RewardSpeedContribution;
    }
    public void UpdateRewardSpeedContribution(float oldContribution, float newContribution)
    {
       Outpostrewardspeed = Outpostrewardspeed + newContribution - oldContribution;
    }
    //IncreaseNpcchage
    public void IncreaseNpcchange(float NpcchangeContribution)
    {
       Npcchange += NpcchangeContribution;
    }
    public void UpdateNpcchangeContribution(float oldContribution, float newContribution)
    {
       Npcchange = Npcchange + newContribution - oldContribution;
    }
    public void CalculateActionSpeed(float factornumber)
    {
        ActionSpeed = 1 + factornumber; 
    }
}
