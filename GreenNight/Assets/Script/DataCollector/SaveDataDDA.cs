using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
[Serializable]
public class DataDDA
{
    public float killPerMinute;
    public float accuracy;
    public int multiKillCount;
    public float barrierDamage;
    public int recordCount;
}
public class SaveDataDDA : MonoBehaviour
{
    public DDAdataCollector scriptDDAdataCollector;
    public DataDDA currentData;
    [SerializeField] string savePathDataDDA;
    // Start is called before the first frame update
    void Start()
    {
        scriptDDAdataCollector = FindObjectOfType<DDAdataCollector>();
    }
    public void AddData()
    {
        float killPerMinute = scriptDDAdataCollector.killPerMinute;
        float accuracy = scriptDDAdataCollector.accuracy;
        int multiKillCount = scriptDDAdataCollector.multiKillCount;
        float barrierDamage = scriptDDAdataCollector.accuracy;

        // หากยังไม่มีข้อมูลเริ่มต้น ให้สร้างใหม่
        if (currentData == null)
        {
            currentData = new DataDDA
            {
                killPerMinute = 0f,
                accuracy = 0f,
                multiKillCount = 0,
                barrierDamage = 0f,
                recordCount = 0
            };
        }

        // บวกค่าปัจจุบัน
        currentData.killPerMinute = (currentData.killPerMinute + killPerMinute) / (currentData.recordCount + 1);
        currentData.accuracy = (currentData.accuracy + accuracy) / (currentData.recordCount + 1);
        currentData.multiKillCount = (currentData.multiKillCount + multiKillCount) / (currentData.recordCount + 1);
        currentData.barrierDamage = (currentData.barrierDamage + barrierDamage) / (currentData.recordCount + 1);

        // เพิ่มจำนวนครั้งที่บันทึกข้อมูล
        currentData.recordCount++;

        // บันทึกข้อมูล
        SaveData();
    }
    // บันทึกข้อมูลลงในไฟล์ JSON
    private void SaveData()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(savePathDataDDA, json);
        Debug.Log($"Data saved to {savePathDataDDA}");
    }

    // โหลดข้อมูลจากไฟล์ JSON
    private void LoadData()
    {
        if (File.Exists(savePathDataDDA))
        {
            string json = File.ReadAllText(savePathDataDDA);
            currentData = JsonUtility.FromJson<DataDDA>(json);
            Debug.Log($"Data loaded from {savePathDataDDA}");
        }
        else
        {
            currentData = new DataDDA();
            Debug.Log("No data file found. Created new data.");
        }
    }

    // รีเซ็ตข้อมูล
    public void ResetData()
    {
        currentData = new DataDDA();
        SaveData();
    }
    // public void SaveDataDDAToJson()
    // {   



    //     var data = JsonConvert.SerializeObject(currentData);
    //     var dataPath = Application.dataPath;
    //     var targetFilePath = Path.Combine(dataPath, savePathDataDDA);

    // }

}
