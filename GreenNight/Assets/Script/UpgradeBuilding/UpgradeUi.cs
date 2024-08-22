using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeUi : MonoBehaviour
{
    public UpgradeBuilding currentBuildingScript;
    public BuildManager buildManager;
    public UImanger uImanger; 
    public Image image;
    public GameObject Waterimage;
    public GameObject Electiciteisimage;
    public GameObject SpecialistImage;
    public TextMeshProUGUI textNameBuild;
    public TextMeshProUGUI textDescriveBuild;
    public TextMeshProUGUI textPlankCost;
    public TextMeshProUGUI textSteelCost;
    public TextMeshProUGUI textNpcCost;
    public TextMeshProUGUI textDayCost;

    void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>();
        uImanger = FindObjectOfType<UImanger>();
    }

    public void Initialize(UpgradeBuilding upgradeBuilding)
    {
        currentBuildingScript = upgradeBuilding;
        SetDataUpgrade();
    }
    public void SetDataUpgrade()
    {
        // textNameBuild.text = building.nameBuild;
        // textDescriveBuild.text = building.detailBuild;
        textPlankCost.text = currentBuildingScript.plankCost.ToString();
        textSteelCost.text = currentBuildingScript.steelCost.ToString();
        textNpcCost.text = currentBuildingScript.npcCost.ToString();
        textDayCost.text = currentBuildingScript.dayCost.ToString();
        // image.sprite = building.GetComponent<SpriteRenderer>().sprite;
        Waterimage.SetActive(currentBuildingScript.isneedwater);
        Electiciteisimage.SetActive(currentBuildingScript.isneedElecticities);
    }
    void OnDisable()
    {
        // Clear the reference when the UI is hidden (optional)
        currentBuildingScript = null;
    }
    public void ComfirmUpgrade()
    {
        if(currentBuildingScript.isneedwater && buildManager.iswateractive)
        {
            if (buildManager.steel >= currentBuildingScript.steelCost && buildManager.plank >= currentBuildingScript.plankCost && buildManager.npc >= currentBuildingScript.npcCost)
            {
                buildManager.steel -= currentBuildingScript.steelCost;
                buildManager.plank -= currentBuildingScript.plankCost;
                buildManager.npc -= currentBuildingScript.npcCost;
                currentBuildingScript.isBuilding = true;
                uImanger.DisableUpgradeUI();
            }
        }
        else if(!currentBuildingScript.isneedwater && buildManager.iswateractive)
        {
            if (buildManager.steel >= currentBuildingScript.steelCost && buildManager.plank >= currentBuildingScript.plankCost && buildManager.npc >= currentBuildingScript.npcCost)
            {
                buildManager.steel -= currentBuildingScript.steelCost;
                buildManager.plank -= currentBuildingScript.plankCost;
                buildManager.npc -= currentBuildingScript.npcCost;
                currentBuildingScript.isBuilding = true;
                uImanger.DisableUpgradeUI();
            }
        }
        else if(currentBuildingScript.isneedwater && !buildManager.iswateractive)
        {
            Debug.Log("Can't upgrade");
        }
    }
}
