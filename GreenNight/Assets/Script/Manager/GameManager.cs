using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Manager Game")]
    public TimeManager timeManager;
    public BuildManager buildManager;
    public InventoryItemPresent inventoryItemPresent;
    public ExpenditionManager expenditionManager;
    public Globalstat globalstat;
    public NpcManager npcManager;
    public ManagerSceneEX managerSceneEX;
    public OutpostSystem outpostSystem;

    [Header("Script Save and Load Game")]
    public SaveAndLoadTimemanager saveAndLoadTimemanager;
    public SaveAndLoadLIstItemsDataBoxes saveAndLoadLIstItemsDataBoxes;
    public SaveAndLoadListNpc saveAndLoadListNpc;
    public SaveAndLoadExpendition saveAndLoadExpendition;
    public SaveDataDDA saveDataDDA;
    public SaveAndLoadListDoorStatusSceneEX saveAndLoadListDoorStatusSceneEX;
    public SaveAndLoadOutPostReward saveAndLoadOutPostReward;
    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        buildManager = FindObjectOfType<BuildManager>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        globalstat = FindObjectOfType<Globalstat>();
        npcManager = FindObjectOfType<NpcManager>();
        managerSceneEX = FindObjectOfType<ManagerSceneEX>();
        outpostSystem = FindObjectOfType<OutpostSystem>();
    }
    public void NewGame()
    {
        saveAndLoadExpendition.ResetDataUIEX();
        saveAndLoadListNpc.ResetDataListNpc();
        saveAndLoadLIstItemsDataBoxes.ResetDataListItemBoxes();
        saveDataDDA.ResetDataDDA();
        saveAndLoadListDoorStatusSceneEX.ResetDataListDoorStatus();
        saveAndLoadOutPostReward.ResetDataOutPostReward();
        saveAndLoadTimemanager.ResetDataTime();
        npcManager.StartGameCreateGropNpx();
    }
    public void SaveGame()
    {
        saveAndLoadListNpc.SaveListNpc();
        saveAndLoadLIstItemsDataBoxes.SaveListItemsDataBoxes();
        saveAndLoadExpendition.SaveUIExpemdition();
        saveAndLoadListDoorStatusSceneEX.SaveDataListDoorStatus();
        saveAndLoadOutPostReward.SaveDataOutPostReward();
        // saveDataDDA.AddDataDDAAndSave();
        saveAndLoadTimemanager.SaveDataTime();
    }
    public void LoadGane()
    {
        saveAndLoadListNpc.LoadDataListNpc();
        saveAndLoadLIstItemsDataBoxes.LoadDataListItemDataBoxes();
        saveAndLoadExpendition.LoadDataUIExFromJsonToScriptData();
        saveDataDDA.LoadDataDDAFromJsonToScriptData();
        saveAndLoadListDoorStatusSceneEX.LoadDataListDoorStatus();
        saveAndLoadOutPostReward.LoadDataOutPostReward();
        saveAndLoadTimemanager.LoadDataTime();
    }
}
