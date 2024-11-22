using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DataDDA
{
    public float killPerMinute;
    public float accuracy;
    public float multiKillCount;
    public float barrierDamage;
    public int recordCount;
}

[Serializable]
public class DataDDACollection
{
    public List<DataDDA> records = new List<DataDDA>();
    public DataDDA averageData = new DataDDA();
}

public class SaveDataDDA : MonoBehaviour
{
    public DDAdataCollector scriptDDAdataCollector;
    public DataDDACollection dataCollection;
    [SerializeField] private string savePathDataDDA;

    private void Start()
    {
        scriptDDAdataCollector = FindObjectOfType<DDAdataCollector>();
      
        LoadData();
    }

    public void AddData()
    {
        // ดึงข้อมูลปัจจุบันจากตัวเก็บข้อมูล
        var newRecord = new DataDDA
        {
            killPerMinute = scriptDDAdataCollector.killPerMinute,
            accuracy = scriptDDAdataCollector.accuracy,
            multiKillCount = scriptDDAdataCollector.multiKillCount,
            barrierDamage = scriptDDAdataCollector.barrierDamage,
            recordCount = dataCollection.records.Count + 1
        };

        // เพิ่มข้อมูลใหม่ลงในรายการ
        dataCollection.records.Add(newRecord);

        // คำนวณค่าเฉลี่ยใหม่
        CalculateAverage();

        // บันทึกข้อมูล
        SaveData();
    }

    private void CalculateAverage()
    {
        // รีเซ็ตค่าเฉลี่ย
        var totalKills = 0f;
        var totalAccuracy = 0f;
        var totalMultiKill = 0f;
        var totalBarrierDamage = 0f;
        var totalCount = dataCollection.records.Count;

        foreach (var record in dataCollection.records)
        {
            totalKills += record.killPerMinute;
            totalAccuracy += record.accuracy;
            totalMultiKill += record.multiKillCount;
            totalBarrierDamage += record.barrierDamage;
        }

        dataCollection.averageData.killPerMinute = totalKills / totalCount;
        dataCollection.averageData.accuracy = totalAccuracy / totalCount;
        dataCollection.averageData.multiKillCount = totalMultiKill / totalCount;
        dataCollection.averageData.barrierDamage = totalBarrierDamage / totalCount;
        dataCollection.averageData.recordCount = totalCount;
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(dataCollection, true);
        File.WriteAllText(savePathDataDDA, json);
        Debug.Log($"Data saved to {savePathDataDDA}");
    }

    private void LoadData()
    {
        if (File.Exists(savePathDataDDA))
        {
            string json = File.ReadAllText(savePathDataDDA);
            dataCollection = JsonUtility.FromJson<DataDDACollection>(json);
            Debug.Log($"Data loaded from {savePathDataDDA}");
        }
        else
        {
            dataCollection = new DataDDACollection();
            Debug.Log("No data file found. Created new data collection.");
        }
    }

    public void ResetData()
    {
        dataCollection = new DataDDACollection();
        SaveData();
    }
}
