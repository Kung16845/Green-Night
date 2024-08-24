using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{   
    public static BuildManager Instance { get; private set; }
    [Header("Resources")]
    public int steel;
    public TextMeshProUGUI steelDisplay;
    public int plank;
    public TextMeshProUGUI plankDisplay;
    public int food;
    public TextMeshProUGUI foodDisplay;
    public int fuel;
    public TextMeshProUGUI fuelDisplay;
    public int ammo;
    public TextMeshProUGUI ammoDisplay;
    public int npc;
    public TextMeshProUGUI npcDisplay;
    [Header("Scipt")]
    public bool iswateractive;
    public bool iselecticitiesactive;
    public Building building;
    public Building buildingToPlace;
    public CustomCursor customCursor;
    public GameObject grid;
    public GameObject uIBuilding;
    public Tile[] tiles;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateResoureDisplay()
    {
        steelDisplay.text = steel.ToString();
        plankDisplay.text = plank.ToString();
        foodDisplay.text = food.ToString();
        fuelDisplay.text = fuel.ToString();
        ammoDisplay.text = ammo.ToString();
        npcDisplay.text = npc.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateResoureDisplay();
        if (buildingToPlace != null)
        {
            // Debug.Log("Check area plce");
            foreach (Tile tile in tiles)
            {
                if (tile.isTileLarge == true && buildingToPlace.isBuildingLarge == true)
                {
                    tile.gameObject.SetActive(true);
                }
                else if (tile.isTileMedium == true && buildingToPlace.isBuildingMedium == true)
                {
                    tile.gameObject.SetActive(true);
                }
                else if (tile.isTileSmall == true && buildingToPlace.isBuildingSmall == true)
                {
                    tile.gameObject.SetActive(true);
                }
                else
                {
                    tile.gameObject.SetActive(false);
                }

            }
        }
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
        {

            Tile nearstTile = null;
            float nearstDistance = float.MaxValue;
            foreach (Tile tile in tiles)
            {
                if (!tile.gameObject.activeSelf)
                {
                    continue; // Skip the rest of the loop if the tile is not active
                }
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < nearstDistance)
                {
                    nearstDistance = dist;
                    nearstTile = tile;
                }
                // Debug.Log("Check area buy");

            }
            if (nearstTile.isOccupied == false && nearstTile.gameObject.activeSelf == true && buildingToPlace != null)
            {
                Instantiate(buildingToPlace, nearstTile.transform.position, Quaternion.identity);
                Debug.Log(nearstTile.name);
                buildingToPlace = null;
                nearstTile.isOccupied = true;
                UIUpdateAfterBuildOrCancelBuild();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {

            if (buildingToPlace != null)
            {
                steel += buildingToPlace.steelCost;
                plank += buildingToPlace.plankCost;
                food += buildingToPlace.foodCost;
                fuel += buildingToPlace.fuelCost;
                ammo += buildingToPlace.ammoCost;
                npc += buildingToPlace.npcCost;
                buildingToPlace = null;
            }
            UIUpdateAfterBuildOrCancelBuild();
        }
    }
    
    public void UIUpdateAfterBuildOrCancelBuild()
    {
        customCursor.gameObject.SetActive(false);
        Cursor.visible = true;
        grid.SetActive(false);
        uIBuilding.SetActive(true);
    }
    public void BuyBuilding()
    {   
        
        if (steel >= building.steelCost && plank >= building.plankCost && food >= building.foodCost
        && fuel >= building.fuelCost && ammo >= building.ammoCost && npc >= building.npcCost)
        {
            steel -= building.steelCost;
            plank -= building.plankCost;
            food -= building.foodCost;
            fuel -= building.fuelCost;
            ammo -= building.ammoCost;
            npc -= building.npcCost;
            // มีภาพลากตามเมาส์ 
            Cursor.visible = false;
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            //
            buildingToPlace = building;
            uIBuilding.SetActive(false);
            grid.SetActive(true);

        }
    }
}
