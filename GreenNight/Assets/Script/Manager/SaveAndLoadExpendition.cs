using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveAndLoadExpendition : MonoBehaviour
{
    public List<DataExpendition> recordExpemdition;
    public List<Transform> listTransformParentUIEx;
    [SerializeField] private string savePathDataExpendition;
    private void Awake()
    {
        savePathDataExpendition = Path.Combine(Application.dataPath, "dataExpendition.json");
    }
    public void SaveUIExpemdition()
    {
        
    }

}

[Serializable]
public class DataExpendition
{
    public int idNPCExpendition;
    List<ItemData> listItemDataInventoryEqicment;
    List<ItemData> listItemDataInventorySlot;
    public float timeScale;
    public float riskEvent;
    public int indexButtonExpendition;
    public int indexSceneExpendition;
    public bool isArriveEx;
    public bool isArriveHome;
    public bool isExpenditon;
    public int finishDayCraftingTime;
    public int finishHourCraftingTime;
    public int finishMinutesCraftingTime;
}
