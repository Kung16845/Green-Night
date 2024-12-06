using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIInventoryEX : UIInventory
{
    public float timeScale;
    public float riskValue;
    public int indexButtonExpendition;
    public int indexSceneExpendition;
    public bool isArriveEx;
    public bool isArriveHome;
    public bool isExpenditon;
    public int finishDayCraftingTime;
    public int finishHourCraftingTime;
    public int finishMinutesCraftingTime;
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
        sceneSystem = FindObjectOfType<SceneSystem>();
        SetPlayerExpendition();
        if (indexButtonExpendition == 1)
        {
            expenditionManager.uIExOne = this.gameObject;
        }
        else if (indexButtonExpendition == 2)
        {
            expenditionManager.uIExTwo = this.gameObject;
        }
    }
    public void SetPlayerExpendition()
    {

        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (isExpenditon)
        {
            Debug.Log("Enter Scene Expendition");
            expenditionManager = FindObjectOfType<ExpenditionManager>();

            inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
            // SetDataForEventExpendition();
            expenditionManager.playerObject = FindObjectOfType<PlayerMovement>().gameObject;
            statAmplifier = FindObjectOfType<StatAmplifier>();

            GameObject npcPlayer = expenditionManager.playerObject;

            npcSelecying = expenditionManager.npcSelecying;
            npcManager = FindObjectOfType<NpcManager>();

            npcManager.dropdown = this.dropdown;
            npcManager.uIInventory = this;
            npcManager.levelCombatText = levelCombatText;
            npcManager.levelEnduranceText = levelEnduranceText;
            npcManager.levelSpeedText = levelSpeedText;
            npcManager.specialistNpcText = specialistNpcText;

            levelCombatText.text = npcSelecying.combat.ToString();
            levelEnduranceText.text = npcSelecying.endurance.ToString();
            levelSpeedText.text = npcSelecying.speed.ToString();
            specialistNpcText.text = npcSelecying.roleNpc.ToString();

            dropdown.ClearOptions();
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = npcSelecying.nameNpc; 

            dropdown.AddOptions(new List<TMP_Dropdown.OptionData> { option });
            spriteHeadNpc.sprite = npcManager.listHeadCoutume.FirstOrDefault(npcCoustume => npcCoustume.idHead == npcSelecying.idHead).spriteHead;

            SetInventoryItemDataEx(expenditionManager.listItemDataInventoryslot, expenditionManager.listItemDataInventoryEqicment);
            SetCostumeNpcExpentdition(npcSelecying, npcPlayer);
            // Recalculate stat amplifiers
            if (statAmplifier != null)
            {
                statAmplifier.endurance = npcSelecying.endurance;
                statAmplifier.combat = npcSelecying.combat;
                statAmplifier.speed = npcSelecying.speed;
                statAmplifier.specialistRole = npcSelecying.roleNpc;
                statAmplifier.InitializeAmplifiers(); // Recalculate multipliers
                // statAmplifier.ApplyRoleModifiers();   // Apply role modifiers
            }
            RefreshUIInventory();
            // expenditionManager.listItemDataInventoryslot.Clear();
            // expenditionManager.listItemDataInventoryEqicment.Clear();
        }
    }
    public void CallFuntionAddListenerButton()
    {
        if (indexButtonExpendition == 1)
        {
            expenditionManager.OpenUIExpenditionInventoryOne();
        }
        else
        {
            expenditionManager.OpenUIExpenditionInventoryTwo();
        }
    }

    public void SetDataMoveSceneForEventExpendition()
    {
        Debug.Log("SetDataMoveSceneForEventExpendition");
        expenditionManager.npcSelecying = this.npcSelecying;
        expenditionManager.listItemDataInventoryEqicment = this.listItemDataInventoryEqicment;
        expenditionManager.listItemDataInventoryslot = this.listItemDataInventorySlot;
    }
    public void SetInventoryItemDataEx(List<ItemData> listDataInventoryslot, List<ItemData> listDataInventoryEqicment)
    {
        listItemDataInventorySlot.Clear();
        listItemDataInventoryEqicment.Clear();
        listItemDataInventorySlot = listDataInventoryslot;
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

        DateTime dateTime = countdownTimeDay.timeManager.dateTime;

        if (dateTime.day <= countdownTimeDay.finishDayCraftingTime)
        {
            npcManager.listNpcWorkingWIthInOneDay.Add(npcSelecying);
        }
        else
        {
            npcManager.listNpcWorkingMoreOneDay.Add(npcSelecying);
        }

        SetUIExButton(countdownTimeDay);

        uINpcSending.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void SetUIExButton(CountdownTimeDay countdownTimeDay)
    {
        if (expenditionManager.uIExOne == null)
        {
            expenditionManager.uIExOne = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXOne.iconComplete;
            indexButtonExpendition = 1;
        }
        else
        {
            expenditionManager.uIExTwo = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXTwo.iconComplete;
            indexButtonExpendition = 2;
        }


        string textdayFinish = "Day : " + finishDayCraftingTime.ToString() + "\n"
        + finishHourCraftingTime.ToString() + ":" + finishMinutesCraftingTime.ToString();
        Sprite spriteHeadNpc = npcManager.listHeadCoutume.FirstOrDefault(head => head.idHead == npcSelecying.idHead).spriteHead;

        expenditionManager.SetUIExButton(indexButtonExpendition, spriteHeadNpc, textdayFinish);
    }
    public void GoExpendition()
    {
        sceneSystem.SwitchScene(indexSceneExpendition);
    }

    public void CancleGoExpenditionAndGoHone()
    {
        CountdownTimeDay countdownTimeDay = expenditionManager.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = timeScale;
        countdownTimeDay.uIInventoryEX = this;
        countdownTimeDay.SetStartExpendition();
        GameObject uIIconComplete = null;

        if (indexButtonExpendition == 1)
        {
            expenditionManager.uIExOne = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXOne.iconComplete;
            uIIconComplete = expenditionManager.uIButtonEXOne.iconComplete;
        }
        else if (indexButtonExpendition == 2)
        {
            expenditionManager.uIExTwo = this.gameObject;
            countdownTimeDay.iconCompleteSend = expenditionManager.uIButtonEXTwo.iconComplete;
            uIIconComplete = expenditionManager.uIButtonEXTwo.iconComplete;
        }

        uIIconComplete.gameObject.SetActive(false);

        Sprite spriteHeadNpc = npcManager.listHeadCoutume.FirstOrDefault(head => head.idHead == npcSelecying.idHead).spriteHead;
        string textdayFinish = "Day : " + countdownTimeDay.finishDayCraftingTime.ToString() + "\n"
        + countdownTimeDay.finishHourCraftingTime.ToString() + ":" + countdownTimeDay.finishMinutesCraftingTime.ToString();

        expenditionManager.SetUIExButton(indexButtonExpendition, spriteHeadNpc, textdayFinish);
        this.gameObject.SetActive(false);
    }
    public void ResetSlotUIEx()
    {


        GameObject uIIconComplete = null;

        if (indexButtonExpendition == 1)
        {
            uIIconComplete = expenditionManager.uIButtonEXOne.iconComplete;
        }
        else if (indexButtonExpendition == 2)
        {
            uIIconComplete = expenditionManager.uIButtonEXTwo.iconComplete;
        }

        uIIconComplete.gameObject.SetActive(false);
        expenditionManager.SetUIExButton(indexButtonExpendition, null, null);
    }
    public void ChoiceLeaveOurSupplies()
    {

        listItemDataInventoryEqicment.Clear();
        listItemDataInventorySlot.Clear();
        RefreshUIInventory();
        Destroy(this.gameObject);
    }
    public void ChoiceFightForIt()
    {
        // เปลีย่นแมพต่อสู้
    }
    public void ChoiceGiveThemHalfourSupplies()
    {
        foreach (ItemData item in listItemDataInventorySlot)
        {
            item.count /= 2;
            if (item.count == 1)
            {
                item.count = 0;
            }
        }
        RefreshUIInventory();
        Destroy(this.gameObject);
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
            if (uINpcGoBack.activeSelf)
            {
                uINpcArriveEx.SetActive(false);
            }
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
                // ResetSlotUIEx();
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
        int gameobjectsceneIndex = gameObject.scene.buildIndex;
        Debug.Log("Scene index Game object : " + gameobjectsceneIndex);
        if (gameobjectsceneIndex != 0)
        {
            return;
        }
        if (indexButtonExpendition == 1)
        {
            expenditionManager.uIExOne = null;
        }
        else
        {
            expenditionManager.uIExTwo = null;
        }
        ResetSlotUIEx();
        expenditionManager.SetUIExButton(indexButtonExpendition, null, null);
        ClearItemDataInAllInventorySlotToListDataBoxes();

        npcManager.listNpc.Add(npcSelecying);
        npcManager.listNpcWorkingMoreOneDay.Remove(npcSelecying);
        npcManager.listNpcWorkingWIthInOneDay.Remove(npcSelecying);
        expenditionManager.listItemDataInventoryEqicment.Clear();
        expenditionManager.listItemDataInventoryslot.Clear();
    }
    public void EndSceneExpendition()
    {

        Debug.Log(" EndSceneExpendition");
        ClearItemDataInAllInventorySlotToListDataBoxes();
        // SetDataMoveSceneForEventExpendition();
        expenditionManager.listItemDataInventoryEqicment.Clear();
        expenditionManager.listItemDataInventoryslot.Clear();
        listItemDataInventoryEqicment.Clear();
        listItemDataInventorySlot.Clear();
        // this.gameObject.SetActive(false);
        // sceneSystem.SwitchScene(0);
    }

}
