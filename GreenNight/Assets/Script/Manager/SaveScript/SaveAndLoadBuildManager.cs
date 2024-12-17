using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;


public class SaveAndLoadBuildManager : MonoBehaviour
{

    public DataColletBuilding dataColletBuilding;
    public BuildManager buildManager;
    public GameManager gameManager;
    [SerializeField] private string saveDataBuildingPath;

    private void Start()
    {
        saveDataBuildingPath = Path.Combine(Application.dataPath, "data_ListBuildinds.json");
        gameManager = FindObjectOfType<GameManager>();
        buildManager = gameManager.buildManager;

        InfoBuildOne infoBuild = new InfoBuildOne();
        infoBuild.transformX = 2;
        infoBuild.transformY = 4;
        infoBuild.nameBuild = "12";
        infoBuild.levelBuild =1;
        infoBuild.dayFinist = 18;
        infoBuild.listNpciD = new List<int>() {0,1,2};
        infoBuild.sizeBuild = "small";

        Debug.Log(dataColletBuilding.listInfoBuilding.ElementAt(0));
        dataColletBuilding.listInfoBuilding.Add(infoBuild);        
        SaveBuildInScenes();
    }
    public void SaveBuildInScenes()
    {
        // AddDataListBuilding();
        string json = JsonUtility.ToJson(dataColletBuilding, true);
        File.WriteAllText(saveDataBuildingPath, json);
    }
    public void AddDataListBuilding()
    {
        foreach (BuiltBuildingInfo build in buildManager.builtBuildings)
        {
            InfoBuilding infoBuilding = new InfoBuilding();

            Building building = build.buildingGameObject.GetComponent<Building>();
            UpgradeBuilding upgradeLevel = building.GetComponent<UpgradeBuilding>();

            infoBuilding.nameBuild = building.name;
            infoBuilding.dayFinist = building.finishDayBuildingTime;

            infoBuilding.transformX = building.transform.position.x;
            infoBuilding.transformY = building.transform.position.y;

            infoBuilding.levelBuild = upgradeLevel.currentLevel;

            dataColletBuilding.listInfoBuilding.Add(infoBuilding);
        }
    }
    public void LoadBuildInScenes()
    {
        if (File.Exists(saveDataBuildingPath))
        {
            string json = File.ReadAllText(saveDataBuildingPath);
            dataColletBuilding = JsonUtility.FromJson<DataColletBuilding>(json);

            foreach (InfoBuilding infoBuilding in dataColletBuilding.listInfoBuilding)
            {
                CreateBuilding(infoBuilding);
            }
        }
        else
        {
            dataColletBuilding = new DataColletBuilding();
        }
    }
    public void CreateBuilding(InfoBuilding infoBuilding)
    {
        GameObject newBuildingObject = buildManager.listALLBuilding.FirstOrDefault(build => build.GetComponent<Building>().nameBuild == infoBuilding.nameBuild);
        Building buildingScript = newBuildingObject.GetComponent<Building>();
        
        buildingScript.nameBuild = infoBuilding.nameBuild;
        buildingScript.finishDayBuildingTime = infoBuilding.dayFinist;

        Vector2 newVector = new Vector2(infoBuilding.transformX, infoBuilding.transformY);
        newBuildingObject.transform.position = newVector;
        newBuildingObject.GetComponent<UpgradeBuilding>().currentLevel = infoBuilding.levelBuild;

        Instantiate(newBuildingObject, newBuildingObject.transform);
    }

}
[Serializable]
public  class DataColletBuilding
{
    public List<InfoBuilding> listInfoBuilding;
}
[Serializable]
public class InfoBuilding
{
    public float transformX;
    public float transformY;
    public string nameBuild;
    public int levelBuild;
    public int dayFinist;
}
[Serializable]
public class InfoBuildOne : InfoBuilding
{
    public string sizeBuild;
    public List<int> listNpciD;
}
[Serializable]
public class InfoBuildTwo : InfoBuilding
{
    public List<int> listNpciD;
}
