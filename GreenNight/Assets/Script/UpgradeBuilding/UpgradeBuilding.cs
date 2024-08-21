using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBuilding : MonoBehaviour
{
    public string nameBuild;
    public string detailBuild;
    public int steelCost;
    public int plankCost;
    public bool isneedwater;
    public bool isneedElecticities;
    // public bool isneedspeciallist;
    public int npcCost;
    public int dayCost;
    public int finishDayBuildingTime;
    public bool isBuilding;
    public TimeManager timeManager;
    public DateTime dateTime;
    public SpriteRenderer spriteRenderer;
    public BuildManager buildManager;
    public GameObject UpgradeUi; // Change to GameObject
    public bool isfinsih;

    void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        dateTime = timeManager.dateTime;
        finishDayBuildingTime += dateTime.day + dayCost;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isBuilding = true;
        isfinsih = false;
    }

    void OnMouseDown()
    {
        if (UpgradeUi != null) // Check if UpgradeUi is assigned
        {
            UpgradeUi.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UpgradeUi is not assigned in the Inspector!");
        }
    }
}
