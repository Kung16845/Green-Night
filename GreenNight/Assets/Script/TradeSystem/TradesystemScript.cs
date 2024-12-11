using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradesystemScript : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    public List<ItemData> listInvenrotyNpcItem;
    public List<InvenrotySlots> listNpcItemWaitforTradeUI = new List<InvenrotySlots>();
    public List<ItemData> listNpcItemWaitforTrade = new List<ItemData>();
    public List<InvenrotySlots> listPlayerItemWaitforTradeUI = new List<InvenrotySlots>();
    public List<ItemData> listPlayerItemWaitforTrade = new List<ItemData>();

    private InventoryItemPresent inventoryItemPresent;
    private ActionController actionController;
    public UIInventory uIInventoryEX;
    public ScriptMoveItems scriptMoveItems;
    public GameObject ConfirmButton;
    public GameObject TradeUI;
    public TextMeshProUGUI statusTrade;
    private bool inrange;
    private static TradesystemScript _instance;

    public static TradesystemScript Instance
    {
        get
        {
            if (_instance == null)
            {
                // Look for an existing TradesystemScript in the scene
                _instance = FindObjectOfType<TradesystemScript>();

                if (_instance == null)
                {
                    Debug.LogError("No TradesystemScript instance found in the scene!");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.LogWarning("Multiple TradesystemScript instances found! Destroying duplicate.");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        actionController = FindObjectOfType<ActionController>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        uIInventoryEX = FindObjectOfType<UIInventory>();
        ConfirmButton.SetActive(false);
        statusTrade.text = "What is your offer?";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = false;
        }
    }

    private void Update()
    {
        if (inrange && Input.GetKeyDown(KeyCode.Tab))
        {
            TradeUI.SetActive(true);
            scriptMoveItems.tradeSystem = this;
            InitializeTradeUI();
        }
        else if (!inrange)
        {
            ClearAllTradeUI();
            TradeUI.SetActive(false);
        }
    }

    private void InitializeTradeUI()
    {
        ClearAllTradeUI();
        foreach (var item in listInvenrotyNpcItem)
        {
            UIItemData prefabData = inventoryItemPresent.listUIItemPrefab.FirstOrDefault(prefab => prefab.idItem == item.idItem);

            if (prefabData != null)
            {
                GameObject newItemUI = Instantiate(prefabData.gameObject, itemsContainer);
                UIItemData uiItemData = newItemUI.GetComponent<UIItemData>();

                ItemClass itemClass = newItemUI.GetComponent<ItemClass>();
                itemClass.nameItem = item.nameItem;
                itemClass.idItem = item.idItem;
                itemClass.quantityItem = item.count;
                itemClass.maxCountItem = item.maxCount;
                itemClass.itemtype = item.itemtype;
                itemClass.tradeValueItem = item.tradeValueItem;

                uiItemData.idItem = item.idItem;
                uiItemData.nameItem = item.nameItem;
                uiItemData.slotTypeParent = SlotType.SlotNpcItem;

                Sprite icon = inventoryItemPresent.GetItemIconByID(item.idItem);
                if (icon != null)
                {
                    uiItemData.itemIconImage.sprite = icon;
                }

                uiItemData.UpdateDataUI(itemClass);
            }
            else
            {
                Debug.LogWarning($"No matching UI prefab found for item ID: {item.idItem}");
            }
        }

        UpdateTradeStatus();
    }
    public void RefreshTrade()
    {
        // Step 1: Clear all child objects in NPC trade UI slots
        foreach (var slot in listNpcItemWaitforTradeUI)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Step 2: Clear all child objects in Player trade UI slots
        foreach (var slot in listPlayerItemWaitforTradeUI)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Step 3: Clear all child objects in the main NPC inventory container
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
        uIInventoryEX.CombineAndSplitItems(listNpcItemWaitforTrade);
        uIInventoryEX.CombineAndSplitItems(listPlayerItemWaitforTrade);
        // Step 4: Populate the NPC inventory UI with items from `listInvenrotyNpcItem`
        foreach (var item in listInvenrotyNpcItem)
        {
            UIItemData prefabData = inventoryItemPresent.listUIItemPrefab.FirstOrDefault(prefab => prefab.idItem == item.idItem);

            if (prefabData != null)
            {
                GameObject newItemUI = Instantiate(prefabData.gameObject, itemsContainer);
                UIItemData uiItemData = newItemUI.GetComponent<UIItemData>();

                ItemClass itemClass = newItemUI.GetComponent<ItemClass>();
                itemClass.nameItem = item.nameItem;
                itemClass.idItem = item.idItem;
                itemClass.quantityItem = item.count;
                itemClass.maxCountItem = item.maxCount;
                itemClass.itemtype = item.itemtype;
                itemClass.tradeValueItem = item.tradeValueItem;

                uiItemData.idItem = item.idItem;
                uiItemData.nameItem = item.nameItem;
                uiItemData.slotTypeParent = SlotType.SlotNpcItem;

                Sprite icon = inventoryItemPresent.GetItemIconByID(item.idItem);
                if (icon != null)
                {
                    uiItemData.itemIconImage.sprite = icon;
                }

                uiItemData.UpdateDataUI(itemClass);
            }
            else
            {
                Debug.LogWarning($"No matching UI prefab found for item ID: {item.idItem}");
            }
        }

        // Step 5: Populate the NPC trade UI with items from `listNpcItemWaitforTrade`
        for (int i = 0; i < listNpcItemWaitforTrade.Count; i++)
        {
            if (i < listNpcItemWaitforTradeUI.Count)
            {
                // Create a UI item for each NPC trade item
                CreateUIItem(listNpcItemWaitforTrade[i], listNpcItemWaitforTradeUI[i]);
            }
        }

        // Step 6: Populate the Player trade UI with items from `listPlayerItemWaitforTrade`
        for (int i = 0; i < listPlayerItemWaitforTrade.Count; i++)
        {
            if (i < listPlayerItemWaitforTradeUI.Count)
            {
                // Create a UI item for each Player trade item
                CreateUIItem(listPlayerItemWaitforTrade[i], listPlayerItemWaitforTradeUI[i]);
            }
        }
    }

    public GameObject CreateUIItem(ItemData itemData, InvenrotySlots invenrotySlots)
    {
        // Get the UI prefab matching the item's ID
        GameObject itemUIPrefab = inventoryItemPresent.listUIItemPrefab
            .FirstOrDefault(prefab => prefab.idItem == itemData.idItem)?.gameObject;

        if (itemUIPrefab == null)
        {
            Debug.LogWarning($"UI prefab not found for item ID: {itemData.idItem}");
            return null;
        }

        // Instantiate the UI item as a child of the given slot
        GameObject itemUICreate = Instantiate(itemUIPrefab, invenrotySlots.transform, true);

        // Set up item class and data
        UIItemData uIItemData = itemUICreate.GetComponent<UIItemData>();
        ItemClass itemClass = itemUICreate.GetComponent<ItemClass>();

        if (itemClass != null)
        {
            itemClass.quantityItem = itemData.count;
            itemClass.maxCountItem = itemData.maxCount;
            itemClass.tradeValueItem = itemData.tradeValueItem; // Set trade value
        }

        if (uIItemData != null)
        {
            uIItemData.slotTypeParent = invenrotySlots.slotTypeInventory;
            uIItemData.UpdateDataUI(itemClass);

            // Optionally display trade value in the UI if needed
            // For example, you can update the item's text or tooltip to show the trade value
        }

        // Return the created UI object
        return itemUICreate;
    }

    public void UpdateTradeLists(SlotType slotType, ItemData itemData, int quantity)
    {
        if (slotType == SlotType.SlotNpcTrade)
        {
            ModifyTradeList(listNpcItemWaitforTrade, itemData, quantity);
        }
        else if (slotType == SlotType.SlotPlayerTrade)
        {
            ModifyTradeList(listPlayerItemWaitforTrade, itemData, quantity);
        }
        RefreshTrade();
        UpdateTradeStatus();
    }

    private void ModifyTradeList(List<ItemData> tradeList, ItemData itemData, int quantity)
    {
        var existingItem = tradeList.FirstOrDefault(item => item.idItem == itemData.idItem);

        if (existingItem != null)
        {
            existingItem.count += quantity;

            if (existingItem.count <= 0)
            {
                tradeList.Remove(existingItem);
            }
        }
        else if (quantity > 0)
        {
            tradeList.Add(new ItemData
            {
                idItem = itemData.idItem,
                nameItem = itemData.nameItem,
                count = quantity,
                maxCount = itemData.maxCount,
                itemtype = itemData.itemtype,
                tradeValueItem = itemData.tradeValueItem
            });
        }
    }

    private void UpdateTradeStatus()
    {
        float npcTradeValue = listNpcItemWaitforTrade.Sum(item => item.tradeValueItem);
        float playerTradeValue = listPlayerItemWaitforTrade.Sum(item => item.tradeValueItem) / 2;

        if (playerTradeValue >= npcTradeValue)
        {
            ConfirmButton.SetActive(true);
            statusTrade.text = playerTradeValue >= 2 * npcTradeValue
                ? "You're too generous. I like that!"
                : playerTradeValue >= 1.5 * npcTradeValue
                ? "How very kind of you."
                : "Fine, we can trade.";
        }
        else
        {
            ConfirmButton.SetActive(false);
            statusTrade.text = playerTradeValue >= 1.1 * npcTradeValue
                ? "A little more, please."
                : playerTradeValue >= 0.75 * npcTradeValue
                ? "Come on, add something more."
                : "Are you kidding? Give me more!";
        }
    }
    private void ClearAllTradeUI()
    {
        // Clear NPC trade slots
        foreach (var slot in listNpcItemWaitforTradeUI)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Clear Player trade slots
        foreach (var slot in listPlayerItemWaitforTradeUI)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Clear the main items container
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        // Clear the trade data lists
        listNpcItemWaitforTrade.Clear();
        listPlayerItemWaitforTrade.Clear();
    }

}
