using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    [Header ("Manager Game")]
    public TimeManager timeManager;
    public BuildManager buildManager;
    public InventoryItemPresent inventoryItemPresent;
    public ExpenditionManager expenditionManager;
    public Globalstat globalstat;
    public NpcManager npcManager;
    
    [Header ("Script Save and Load Game")]
    public SaveAndLoadListNpc saveAndLoadListNpc;
    public SaveAndLoadExpendition saveAndLoadExpendition;
    public SaveDataDDA saveDataDDA;
    private void Awake() 
    {
        npcManager = FindObjectOfType<NpcManager>();
    }
    public void NewGame()
    {
        npcManager.StartGameCreateGropNpx();
    }
    public void SaveGame()
    {
        saveAndLoadListNpc.SaveListNpc();
        saveAndLoadExpendition.SaveUIExpemdition();
    }
    public void LoadGane()
    {        
        saveAndLoadListNpc.LoadDataListNpc();
        saveAndLoadExpendition.LoadDataUIExFromJsonToScriptData();
    }
}
