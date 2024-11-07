using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIInventoryEX : UIInventory
{
    public float timeScale;
    public int indexExpendition;
    public bool isArrive;
    public ExpenditionManager expenditionManager;
    public SceneSystem sceneSystem;
    public Button button1;
    private void Awake()
    {
        SetValuableUIInventory();
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        sceneSystem = FindObjectOfType<SceneSystem>();
    }
    public void Start()
    {
        expenditionManager = FindObjectOfType<ExpenditionManager>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        button1.onClick.AddListener(CallFuntionAddListenerButton);
        if (currentSceneIndex == 2)
        {
            Debug.Log("Enter Scene Expendition");

            // SetDataForEventExpendition();
            expenditionManager.playerObject = FindObjectOfType<PlayerMovement>().gameObject;
            GameObject npcPlayer = expenditionManager.playerObject;
            SetInventoryItemDataEx(expenditionManager.listItemDataInventoryslot, expenditionManager.listItemDataInventoryEqicment);
            SetCostumeNpcExpentdition(npcSelecying, npcPlayer);
            // expenditionManager.listItemDataInventoryslot.Clear();
            // expenditionManager.listItemDataInventoryEqicment.Clear();

        }

        if (indexExpendition == 1)
        {
            expenditionManager.uIExOne = this.gameObject;
        }
        else if (indexExpendition == 2)
        {
            expenditionManager.uIExTwo = this.gameObject;
        }
    }
    public void CallFuntionAddListenerButton()
    {
        if (indexExpendition == 1)
        {
            expenditionManager.OpenUIExpenditionInventoryOne();
        }
        else
        {
            expenditionManager.OpenUIExpenditionInventoryTwo();
        }
    }
    public void Update()
    {

    }
    public void SetDataMoveSceneForEventExpendition()
    {

        expenditionManager.npcSelecying = this.npcSelecying;
        expenditionManager.listItemDataInventoryEqicment = this.listItemDataInventoryEqicment;
        expenditionManager.listItemDataInventoryslot = this.listItemDataInventoryslot;
    }
    public void SetInventoryItemDataEx(List<ItemData> listDataInventoryslot, List<ItemData> listDataInventoryEqicment)
    {
        listItemDataInventoryslot.Clear();
        listItemDataInventoryEqicment.Clear();
        listItemDataInventoryslot = listDataInventoryslot;
        listItemDataInventoryEqicment = listDataInventoryEqicment;
        RefreshUIInventory();
    }
    public void SendNpcExpendition()
    {
        CountdownTimeDay countdownTimeDay = expenditionManager.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = timeScale;
        countdownTimeDay.uIInventoryEX = this;
        countdownTimeDay.SetStartExpendition();

        npcManager.listNpc.Remove(npcSelecying);

        Sprite spriteHeadNpc = npcManager.listHeadCoutume.FirstOrDefault(head => head.idHead == npcSelecying.idHead).spriteHead;
        DateTime dateTime = countdownTimeDay.timeManager.dateTime;

        if (dateTime.day <= countdownTimeDay.finishDayCraftingTime)
        {
            npcManager.listNpcWorkingWIthInOneDay.Add(npcSelecying);
        }
        else
        {
            npcManager.listNpcWorkingMoreOneDay.Add(npcSelecying);
        }

        if (expenditionManager.uIExOne == null)
        {
            expenditionManager.uIExOne = this.gameObject;
            indexExpendition = 1;
        }
        else
        {
            expenditionManager.uIExTwo = this.gameObject;
            indexExpendition = 2;
        }

        string textdayFinish = "Day : " + countdownTimeDay.finishDayCraftingTime.ToString() + "\n"
        + countdownTimeDay.finishHourCraftingTime.ToString() + ":" + countdownTimeDay.finishMinutesCraftingTime.ToString();

        expenditionManager.SetUIExButton(indexExpendition, spriteHeadNpc, textdayFinish);

        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        RefreshUIInventory();

        if (isArrive)
        {
            SetDataMoveSceneForEventExpendition();
            Destroy(this.gameObject, 2f);
            sceneSystem.SwitchScene(2);
        }
    }
    private void OnDisable()
    {
        ConventDataUIToItemData();
    }
    private void OnDestroy()
    {
        if (indexExpendition == 1)
        {
            expenditionManager.uIExOne = null;
        }
        else
        {
            expenditionManager.uIExTwo = null;
        }

        expenditionManager.SetUIExButton(0, null, null);

    }
    public void EndSceneExpendition()
    {
        ClearItemDataInAllInventorySlotToListDataBoxes();
        SetDataMoveSceneForEventExpendition();
        listItemDataInventoryEqicment.Clear();
        listItemDataInventoryslot.Clear();
        sceneSystem.SwitchScene(0);
    }

}
