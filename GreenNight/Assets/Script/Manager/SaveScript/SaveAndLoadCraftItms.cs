using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveAndLoadCraftItms : MonoBehaviour
{
    public GameManager gameManager;
    public CraftManager craftManager;
    public DataCollentCraftItems dataCollentCraftItems;
    [SerializeField] private string savePathDataCraftItmes;
    private void Start()
    {
        savePathDataCraftItmes = Path.Combine(Application.dataPath, "dataCraftItms.json");
        gameManager = FindObjectOfType<GameManager>();
        craftManager = gameManager.craftManager;
    }
    public void SaveDataCraftItems()
    {
        AddDataCraftItems();
        string json = JsonUtility.ToJson(dataCollentCraftItems, true);
        File.WriteAllText(savePathDataCraftItmes, json);
    }
    public void AddDataCraftItems()
    {
        dataCollentCraftItems = new DataCollentCraftItems();

        foreach (CraftingJob itemCraftJobs in craftManager.activeCraftingJobs)
        {
            DataItemsCraft dataItemsCraft = new DataItemsCraft();

            dataItemsCraft.idItem = itemCraftJobs.craftingItem.itemID;
            dataItemsCraft.timeRemaining = itemCraftJobs.timeRemaining;
            dataItemsCraft.isComplete = itemCraftJobs.isComplete;
            dataItemsCraft.numCraftingSource = (int)itemCraftJobs.source;

            dataCollentCraftItems.listActiveCraftingJobs.Add(dataItemsCraft);
        }

        foreach (CraftingJob itemChemical in craftManager.ChemicalactiveJobs)
        {
            DataItemsCraft dataItemsCraft = new DataItemsCraft();

            dataItemsCraft.idItem = itemChemical.craftingItem.itemID;
            dataItemsCraft.timeRemaining = itemChemical.timeRemaining;
            dataItemsCraft.isComplete = itemChemical.isComplete;
            dataItemsCraft.numCraftingSource = (int)itemChemical.source;

            dataCollentCraftItems.listChemicalactiveJobs.Add(dataItemsCraft);
        }

        foreach (CraftingJob itemMedicine in craftManager.MedicineactiveJobs)
        {
            DataItemsCraft dataItemsCraft = new DataItemsCraft();

            dataItemsCraft.idItem = itemMedicine.craftingItem.itemID;
            dataItemsCraft.timeRemaining = itemMedicine.timeRemaining;
            dataItemsCraft.isComplete = itemMedicine.isComplete;
            dataItemsCraft.numCraftingSource = (int)itemMedicine.source;

            dataCollentCraftItems.listMedicineactiveJobs.Add(dataItemsCraft);
        }
    }
    public void LoadDataCraftItems()
    {
        if (File.Exists(savePathDataCraftItmes))
        {
            string json = File.ReadAllText(savePathDataCraftItmes);
            dataCollentCraftItems = JsonUtility.FromJson<DataCollentCraftItems>(json);

            if (dataCollentCraftItems == null)
            {
                dataCollentCraftItems = new DataCollentCraftItems();
            }
            if (dataCollentCraftItems.listActiveCraftingJobs == null)
            {
                dataCollentCraftItems.listActiveCraftingJobs = new List<DataItemsCraft>();
            }
            if (dataCollentCraftItems.listMedicineactiveJobs == null)
            {
                dataCollentCraftItems.listMedicineactiveJobs = new List<DataItemsCraft>();
            }
            if (dataCollentCraftItems.listChemicalactiveJobs == null)
            {
                dataCollentCraftItems.listChemicalactiveJobs = new List<DataItemsCraft>();
            }

            if (dataCollentCraftItems.listActiveCraftingJobs.Count > 0)
            {
                foreach (DataItemsCraft dataItem in dataCollentCraftItems.listActiveCraftingJobs)
                {
                    CraftingItem newCraftingItem = new CraftingItem();

                    CraftingJob newCraftingItemJob = new CraftingJob(newCraftingItem, dataItem.timeRemaining
                    , (CraftingSource)dataItem.numCraftingSource);

                    craftManager.activeCraftingJobs.Add(newCraftingItemJob);
                }
            }

            if (dataCollentCraftItems.listChemicalactiveJobs.Count > 0)
            {
                foreach (DataItemsCraft dataItem in dataCollentCraftItems.listChemicalactiveJobs)
                {
                    CraftingItem newCraftingItem = new CraftingItem();

                    CraftingJob newCraftingItemJob = new CraftingJob(newCraftingItem, dataItem.timeRemaining
                    , (CraftingSource)dataItem.numCraftingSource);

                    craftManager.ChemicalactiveJobs.Add(newCraftingItemJob);
                }
            }

            if (dataCollentCraftItems.listMedicineactiveJobs.Count > 0)
            {
                foreach (DataItemsCraft dataItem in dataCollentCraftItems.listMedicineactiveJobs)
                {
                    CraftingItem newCraftingItem = new CraftingItem();

                    CraftingJob newCraftingItemJob = new CraftingJob(newCraftingItem, dataItem.timeRemaining
                    , (CraftingSource)dataItem.numCraftingSource);

                    craftManager.MedicineactiveJobs.Add(newCraftingItemJob);
                }
            }
        }
        else
        {
            dataCollentCraftItems = new DataCollentCraftItems();
            dataCollentCraftItems.listActiveCraftingJobs = new List<DataItemsCraft>();
            dataCollentCraftItems.listMedicineactiveJobs = new List<DataItemsCraft>();
            dataCollentCraftItems.listChemicalactiveJobs = new List<DataItemsCraft>();
        }

    }
    public void ResetDataCraftItems()
    {
        dataCollentCraftItems = new DataCollentCraftItems();
        dataCollentCraftItems.listActiveCraftingJobs = new List<DataItemsCraft>();
        dataCollentCraftItems.listMedicineactiveJobs = new List<DataItemsCraft>();
        dataCollentCraftItems.listChemicalactiveJobs = new List<DataItemsCraft>();
        string json = JsonUtility.ToJson(dataCollentCraftItems, true);
        File.WriteAllText(savePathDataCraftItmes, json);
    }
}
[Serializable]
public class DataCollentCraftItems
{
    public List<DataItemsCraft> listActiveCraftingJobs;
    public List<DataItemsCraft> listChemicalactiveJobs;
    public List<DataItemsCraft> listMedicineactiveJobs;
}
[Serializable]
public class DataItemsCraft
{
    public int idItem;
    public float timeRemaining;
    public bool isComplete;
    public int numCraftingSource;
}
