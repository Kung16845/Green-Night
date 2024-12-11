using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class SaveAndLoadLIstItemsDataBoxes : MonoBehaviour
{
    public DataCollentListItemsBoxes dataCollentListItemsBoxes;
    public GameManager gameManager;
    public InventoryItemPresent inventoryItemPresent;
    [SerializeField] private string savePathDataListItemsBoxes;
    private void Start()
    {
        savePathDataListItemsBoxes = Path.Combine(Application.dataPath, "dataListItemsDataBoxes.json");
        gameManager = FindObjectOfType<GameManager>();
        inventoryItemPresent = gameManager.inventoryItemPresent;
    }
    public void SaveListItemsDataBoxes()
    {
        dataCollentListItemsBoxes.listItemBoxes = inventoryItemPresent.listItemsDataBox;
        string json = JsonUtility.ToJson(dataCollentListItemsBoxes,true);
        File.WriteAllText(savePathDataListItemsBoxes,json);
    }
    public void LoadDataListItemDataBoxes()
    {
        if(File.Exists(savePathDataListItemsBoxes))
        {
            string json = File.ReadAllText(savePathDataListItemsBoxes);
            dataCollentListItemsBoxes = JsonUtility.FromJson<DataCollentListItemsBoxes>(json);
            inventoryItemPresent.listItemsDataBox = dataCollentListItemsBoxes.listItemBoxes;
        }
        else 
        {
            dataCollentListItemsBoxes = new DataCollentListItemsBoxes();
        }
    }
    public void ResetDataListItemBoxes()
    {
        dataCollentListItemsBoxes = new DataCollentListItemsBoxes();
        string json = JsonUtility.ToJson(dataCollentListItemsBoxes,true);
        File.WriteAllText(savePathDataListItemsBoxes,json);
    }
}
[Serializable]
public class DataCollentListItemsBoxes
{
    public List<ItemData> listItemBoxes;
}
