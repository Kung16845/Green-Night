using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public SaveAndLoadBuildManager saveAndLoadBuildManager;
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
        buildManager = FindObjectOfType<BuildManager>();
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
        saveAndLoadBuildManager.ResetDataBuilding();
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
        saveAndLoadBuildManager.SaveBuildInScenes();
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
        saveAndLoadBuildManager.LoadBuildInScenes();
    }
    public void QuitGame()
    {
        
        Application.Quit();
        
    }
}
