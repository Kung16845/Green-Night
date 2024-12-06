using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class SaveAndLoadExpendition : MonoBehaviour
{
    public DataCollentUIEX dataCollentUIEX;
    public Transform transformParentUIEx;
    public GameManager gameManager;
    [SerializeField] private string savePathDataExpendition;
    private void Awake()
    {
        transformParentUIEx = gameManager.expenditionManager.transformsUIEx;
        savePathDataExpendition = Path.Combine(Application.dataPath, "dataExpendition.json");
    }
    public void SaveUIExpemdition()
    {

        string json = JsonUtility.ToJson(dataCollentUIEX, true);
        File.WriteAllText(savePathDataExpendition, json);
    }
    public void AddDataBeforeSaveToJaon()
    {
        if (transformParentUIEx.childCount == 0) return;

        UIInventoryEX[] listUIEX = transformParentUIEx.GetComponentsInChildren<UIInventoryEX>();

        foreach (UIInventoryEX uIEx in listUIEX)
        {
            DataSaveExpendition dataExpenditionSave = new DataSaveExpendition();

            dataExpenditionSave.idNPCExpendition = uIEx.npcSelecying.idnpc;

            dataExpenditionSave.listItemDataInventoryEqicment = uIEx.listItemDataInventoryEqicment;
            dataExpenditionSave.listItemDataInventorySlot = uIEx.listItemDataInventorySlot;

            dataExpenditionSave.timeScale = uIEx.timeScale;
            dataExpenditionSave.riskEventValue = uIEx.riskValue;

            dataExpenditionSave.indexButtonExpendition = uIEx.indexButtonExpendition;
            dataExpenditionSave.indexSceneExpendition = uIEx.indexSceneExpendition;

            dataExpenditionSave.isArriveEx = uIEx.isArriveEx;
            dataExpenditionSave.isArriveHome = uIEx.isArriveHome;
            dataExpenditionSave.isExpenditon = uIEx.isExpenditon;

            dataExpenditionSave.finishDayCraftingTime = uIEx.finishDayCraftingTime;
            dataExpenditionSave.finishHourCraftingTime = uIEx.finishHourCraftingTime;
            dataExpenditionSave.finishMinutesCraftingTime = uIEx.finishMinutesCraftingTime;

            dataCollentUIEX.listdataUIExpemdition.Add(dataExpenditionSave);
        }

    }
    public void LoadDataUIExFromJsonToScriptData()
    {
        if (File.Exists(savePathDataExpendition))
        {
            string json = File.ReadAllText(savePathDataExpendition);
            dataCollentUIEX = JsonUtility.FromJson<DataCollentUIEX>(json);
            Debug.Log($"Data loaded from {savePathDataExpendition}");
        }
        else
        {
            dataCollentUIEX = new DataCollentUIEX();
            Debug.Log("No data file found. Created new data collection.");
        }
    }
    public void CreateUIEX(DataSaveExpendition dataSaveExpendition)
    {
        ExpenditionManager expenditionManager = gameManager.expenditionManager;
        GameObject uIEx = Instantiate(expenditionManager.uIInventoryExPrefab, transformParentUIEx);
        UIInventoryEX newUIInventoryEX = uIEx.GetComponent<UIInventoryEX>();

        newUIInventoryEX.npcSelecying = gameManager.npcManager.listNpc.FirstOrDefault(npc => npc.idnpc == dataSaveExpendition.idNPCExpendition);

        newUIInventoryEX.listItemDataInventoryEqicment = dataSaveExpendition.listItemDataInventoryEqicment;
        newUIInventoryEX.listItemDataInventorySlot = dataSaveExpendition.listItemDataInventorySlot;

        newUIInventoryEX.timeScale = dataSaveExpendition.timeScale;
        newUIInventoryEX.riskValue = dataSaveExpendition.riskEventValue;

        newUIInventoryEX.indexButtonExpendition = dataSaveExpendition.indexButtonExpendition;
        newUIInventoryEX.indexSceneExpendition = dataSaveExpendition.indexSceneExpendition;

        newUIInventoryEX.isArriveEx = dataSaveExpendition.isArriveEx;
        newUIInventoryEX.isArriveHome = dataSaveExpendition.isArriveHome;
        newUIInventoryEX.isExpenditon = dataSaveExpendition.isExpenditon;

        newUIInventoryEX.finishDayCraftingTime = dataSaveExpendition.finishDayCraftingTime;
        newUIInventoryEX.finishHourCraftingTime = dataSaveExpendition.finishHourCraftingTime;
        newUIInventoryEX.finishMinutesCraftingTime = dataSaveExpendition.finishMinutesCraftingTime;

        CountdownTimeDay countdownTimeDay = expenditionManager.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = newUIInventoryEX.timeScale;
        countdownTimeDay.uIInventoryEX = newUIInventoryEX;
        countdownTimeDay.finishDayCraftingTime = newUIInventoryEX.finishDayCraftingTime;
        countdownTimeDay.finishHourCraftingTime = newUIInventoryEX.finishHourCraftingTime;
        countdownTimeDay.finishMinutesCraftingTime = newUIInventoryEX.finishMinutesCraftingTime;

    }
}
[Serializable]
public class DataCollentUIEX
{
    public List<DataSaveExpendition> listdataUIExpemdition;
}
[Serializable]
public class DataSaveExpendition
{
    public int idNPCExpendition;
    public List<ItemData> listItemDataInventoryEqicment;
    public List<ItemData> listItemDataInventorySlot;
    public float timeScale;
    public float riskEventValue;
    public int indexButtonExpendition;
    public int indexSceneExpendition;
    public bool isArriveEx;
    public bool isArriveHome;
    public bool isExpenditon;
    public int finishDayCraftingTime;
    public int finishHourCraftingTime;
    public int finishMinutesCraftingTime;
}
