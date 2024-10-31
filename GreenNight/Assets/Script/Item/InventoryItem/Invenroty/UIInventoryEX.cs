using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryEX : UIInventory
{
    public float timeScale;
    public int indexExpendition;
    public bool isArrive;
    public ExpenditionManager expenditionManager;
    public SceneSystem sceneSystem;
    private void Awake()
    {
        SetValuableUIInventory();
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        sceneSystem = FindObjectOfType<SceneSystem>();
    }
    public void Start()
    {
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        if (indexExpendition == 1)
        {
            expenditionManager.uIExOne = this.gameObject;
        }
        else if (indexExpendition == 2)
        {
            expenditionManager.uIExTwo = this.gameObject;
        }
    }
    public void Update()
    {
        if (isArrive)
        {
            // Destroy(this.gameObject);
        }
    }
    public void SendNpcExpendition()
    {
        CountdownTimeDay countdownTimeDay = this.gameObject.AddComponent<CountdownTimeDay>();
        countdownTimeDay.timeScale = timeScale;
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

        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        RefreshUIInventory();
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
        
        SetDataForEventExpendition();
    }
    public void SetDataForEventExpendition()
    {   
        expenditionManager.npcSelecying = this.npcSelecying;
        expenditionManager.listItemDataInventoryEqicment = this.listItemDataInventoryEqicment;
        expenditionManager.listItemDataInventoryslot = this.listItemDataInventoryslot;
    }
    public void AddITemlistInvenrotySlots(ItemClass itemClass)
    {
        ItemData itemData = listItemDataInventoryslot.FirstOrDefault(item => item.idItem == itemClass.idItem);
        if (listItemDataInventoryslot.Count < npcSelecying.countInventorySlot)
        {
            if (itemData != null)
            {
                if (itemData.count + itemClass.quantityItem <= itemData.maxCount)
                {
                    itemData.count += itemClass.quantityItem;
                }
                else
                {
                    ItemData newitemData = itemData;
                    newitemData.count = itemClass.quantityItem - (itemData.maxCount - itemData.count);
                    listItemDataInventoryslot.Add(newitemData);
                    itemData.count = itemData.maxCount;
                }
            }
            else
            {
                listItemDataInventoryslot.Add(itemData);
            }
        }
        else
        {
            if (itemData != null)
            {
                itemData.count = itemClass.maxCountItem;
            }
            else
                return;
        }
    }
}
