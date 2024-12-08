using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class SaveAndLoadListNpc : MonoBehaviour
{
    public DataCollentListNpc dataCollentListNpc;
    public GameManager gameManager;
    [SerializeField] private string savePathDataListNpc;
    // Start is called before the first frame update
    void Start()
    {
        savePathDataListNpc = Path.Combine(Application.dataPath, "datalistNpc.json");
    }
    public void SaveListNpc()
    {
        string json = JsonUtility.ToJson(dataCollentListNpc, true);
        File.WriteAllText(savePathDataListNpc, json);
    }
    
}

[Serializable]
public class DataCollentListNpc
{
    public List<NpcClass> listDataNPC;
    public List<NpcClass> listDataNPCWorkInOneDay;
    public List<NpcClass> listDataNPCWorkInMoreOneDay;
}
