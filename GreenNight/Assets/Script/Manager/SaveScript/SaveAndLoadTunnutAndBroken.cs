using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveAndLoadTunnutAndBroken : MonoBehaviour
{
    public GameManager gameManager;
    public Tunnel tunnel;
    public Sattlelite sattlelite;
    public DataCollentTunnutAndBroken dataCollentTunnutAndBroken;
    [SerializeField] private string savePathDataTunnutAndBroken;
    private void Start()
    {
        savePathDataTunnutAndBroken = Path.Combine(Application.dataPath, "dataTunnutAndBroken .json");
        gameManager = FindObjectOfType<GameManager>();

    }
    public void SaveDataTime()
    {
        AddDataColletTime();
        string json = JsonUtility.ToJson(dataCollentTunnutAndBroken, true);
        File.WriteAllText(savePathDataTunnutAndBroken, json);
    }
    public void AddDataColletTime()
    {

    }
    public void LoadDataTime()
    {
        if (File.Exists(savePathDataTunnutAndBroken))
        {
            string json = File.ReadAllText(savePathDataTunnutAndBroken);
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
        File.WriteAllText(savePathDataTunnutAndBroken, json);
    }
}
[SerializeField]
public class DataCollentTunnutAndBroken
{

}