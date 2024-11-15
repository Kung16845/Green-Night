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
    public float riskValue;
    public int indexExpendition;
    public bool isArriveEx;
    public bool isArriveHome;
    public ExpenditionManager expenditionManager;
    public SceneSystem sceneSystem;
    public GameObject uINpcSending;
    public GameObject uINpcArriveEx;
    public GameObject uINpcGoBack;
    public List<GameObject> listEvnet;
    private void Awake()
    {
        SetValuableUIInventory();
        expenditionManager = FindObjectOfType<ExpenditionManager>();

    }
    public void Start()
    {
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        sceneSystem = FindObjectOfType<SceneSystem>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2)
        {
            Debug.Log("Enter Scene Expendition");

            // SetDataForEventExpendition();
            expenditionManager.playerObject = FindObjectOfType<PlayerMovement>().gameObject;
            statAmplifier = FindObjectOfType<StatAmplifier>();
            GameObject npcPlayer = expenditionManager.playerObject;

            SetInventoryItemDataEx(expenditionManager.listItemDataInventoryslot, expenditionManager.listItemDataInventoryEqicment);
            SetCostumeNpcExpentdition(npcSelecying, npcPlayer);
            // Recalculate stat amplifiers
            if (statAmplifier != null)
            {
                statAmplifier.InitializeAmplifiers(); // Recalculate multipliers
                statAmplifier.ApplyRoleModifiers();   // Apply role modifiers
            }
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
        Debug.Log("SetDataMoveSceneForEventExpendition");
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
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXOne.iconComplete;
            indexExpendition = 1;
        }
        else
        {
            expenditionManager.uIExTwo = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXTwo.iconComplete;
            indexExpendition = 2;
        }

        string textdayFinish = "Day : " + countdownTimeDay.finishDayCraftingTime.ToString() + "\n"
        + countdownTimeDay.finishHourCraftingTime.ToString() + ":" + countdownTimeDay.finishMinutesCraftingTime.ToString();

        expenditionManager.SetUIExButton(indexExpendition, spriteHeadNpc, textdayFinish);

        uINpcSending.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void GoExpendition()
    {
        sceneSystem.SwitchScene(2);
    }

    public void CancleGoExpenditionAndGoHone()
    {
        CountdownTimeDay countdownTimeDay = expenditionManager.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = timeScale;
        countdownTimeDay.uIInventoryEX = this;
        countdownTimeDay.SetStartExpendition();
        GameObject uIIconComplete = null;

        if (indexExpendition == 1)
        {
            expenditionManager.uIExOne = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXOne.iconComplete;
            uIIconComplete = expenditionManager.uIButtonEXOne.iconComplete;
        }
        else if (indexExpendition == 2)
        {
            expenditionManager.uIExTwo = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXTwo.iconComplete;
            uIIconComplete = expenditionManager.uIButtonEXTwo.iconComplete;
        }

        uIIconComplete.gameObject.SetActive(false);

        Sprite spriteHeadNpc = npcManager.listHeadCoutume.FirstOrDefault(head => head.idHead == npcSelecying.idHead).spriteHead;
        string textdayFinish = "Day : " + countdownTimeDay.finishDayCraftingTime.ToString() + "\n"
        + countdownTimeDay.finishHourCraftingTime.ToString() + ":" + countdownTimeDay.finishMinutesCraftingTime.ToString();

        expenditionManager.SetUIExButton(indexExpendition, spriteHeadNpc, textdayFinish);
        this.gameObject.SetActive(false);
    }
    public void ResetSlotUIEx()
    {
        expenditionManager.SetUIExButton(indexExpendition, null, null);
        GameObject uIIconComplete = null;

        if (indexExpendition == 1)
        {   
            uIIconComplete = expenditionManager.uIButtonEXOne.iconComplete;
        }
        else if (indexExpendition == 2)
        {
            uIIconComplete = expenditionManager.uIButtonEXTwo.iconComplete;
        }

        uIIconComplete.gameObject.SetActive(false);
    }
    public bool IsEventTriggered()
    {
        float randomValue = Random.Range(0f, 100f); // สุ่มตัวเลขระหว่าง 0 ถึง 100
        return randomValue < riskValue; // คืนค่า true ถ้า randomValue น้อยกว่า riskValue
    }
    private void OnEnable()
    {
        RefreshUIInventory();

        if (isArriveEx && !isArriveHome)
        {
            SetDataMoveSceneForEventExpendition();
            uINpcSending.SetActive(false);
            uINpcArriveEx.SetActive(true);
            // sceneSystem.SwitchScene(2);
            // Destroy(this.gameObject, 2f);
        }
        else if (isArriveHome && isArriveEx)
        {
            if (IsEventTriggered())
            {
                Debug.Log("Found Event");
                uINpcGoBack.SetActive(false);
                int randomValue = Random.Range(0, 2);
                listEvnet.ElementAt(randomValue).gameObject.SetActive(true);
            }
            else
            {
                ResetSlotUIEx();
                Destroy(this.gameObject);
            }
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
        ClearItemDataInAllInventorySlotToListDataBoxes();
    }
    public void EndSceneExpendition()
    {

        Debug.Log(" EndSceneExpendition");
        ClearItemDataInAllInventorySlotToListDataBoxes();
        // SetDataMoveSceneForEventExpendition();
        expenditionManager.listItemDataInventoryEqicment.Clear();
        expenditionManager.listItemDataInventoryslot.Clear();
        listItemDataInventoryEqicment.Clear();
        listItemDataInventoryslot.Clear();
        // this.gameObject.SetActive(false);
        // sceneSystem.SwitchScene(0);
    }

}
