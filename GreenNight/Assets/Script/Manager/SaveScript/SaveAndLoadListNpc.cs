using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class SaveAndLoadListNpc : MonoBehaviour
{
    public DataCollentListNpc dataCollentListNpc;
    public GameManager gameManager;
    public NpcManager npcManager;
    [SerializeField] private string savePathDataListNpc;
    // Start is called before the first frame update
    void Start()
    {
        savePathDataListNpc = Path.Combine(Application.dataPath, "datalistNpc.json");
        npcManager = gameManager.npcManager;
    }
    public void SaveListNpc()
    {
        string json = JsonUtility.ToJson(dataCollentListNpc, true);
        File.WriteAllText(savePathDataListNpc, json);
    }
    public void AddDataCollectListNpc()
    {

        dataCollentListNpc.listDataNPC = npcManager.listNpc;
        dataCollentListNpc.listDataNPCWorking = npcManager.listNpcWorking;
    }
    public void LoadDataListNpc()
    {
        if (File.Exists(savePathDataListNpc))
        {
            string json = File.ReadAllText(savePathDataListNpc);
            dataCollentListNpc = JsonUtility.FromJson<DataCollentListNpc>(json);
            SetListDataNpc();
        }
        else
        {
            dataCollentListNpc = new DataCollentListNpc();
        }

    }
    public void SetListDataNpc()
    {
        npcManager.listNpc = dataCollentListNpc.listDataNPC;
        npcManager.listNpcWorking = dataCollentListNpc.listDataNPCWorking;
    }
}

[Serializable]
public class DataCollentListNpc
{
    public List<NpcClass> listDataNPC;
    public List<NpcClass> listDataNPCWorking;
}

