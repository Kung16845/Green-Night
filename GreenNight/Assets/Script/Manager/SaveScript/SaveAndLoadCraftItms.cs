using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoadCraftItms : MonoBehaviour
{
    public GameManager gameManager;
    public Tunnel tunnel;
    public Sattlelite sattlelite;
    public DataCollentTunnutAndBroken dataCollentTunnutAndBroken;
    [SerializeField] private string savePathDataCraftItmes;
    private void Start()
    {
        savePathDataCraftItmes = Path.Combine(Application.dataPath, "dataTunnutAndBroken .json");
        gameManager = FindObjectOfType<GameManager>();

    }
    public void SaveDataTime()
    {
        AddDataColletTime();
        string json = JsonUtility.ToJson(dataCollentTunnutAndBroken, true);
        File.WriteAllText(savePathDataCraftItmes, json);
    }
    public void AddDataColletTime()
    {

    }
    public void LoadDataTime()
    {
        if (File.Exists(savePathDataCraftItmes))
        {
            string json = File.ReadAllText(savePathDataCraftItmes);
            dataCollentTunnutAndBroken = JsonUtility.FromJson<DataCollentTunnutAndBroken>(json);

            // timeManager.dateTime.sceneSystem = FindObjectOfType<SceneSystem>();

        }
        else
        {
            dataCollentTunnutAndBroken = new DataCollentTunnutAndBroken();
        }

    }
    public void ResetDataTime()
    {
        dataCollentTunnutAndBroken = new DataCollentTunnutAndBroken();
        string json = JsonUtility.ToJson(dataCollentTunnutAndBroken, true);
        File.WriteAllText(savePathDataCraftItmes, json);
    }
}
