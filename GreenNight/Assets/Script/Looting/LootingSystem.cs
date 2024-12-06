using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening; 
using System.Linq;

public class LootingSystem : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer; // The UI container where loot items will be displayed
    [SerializeField] private GameObject lootItemUIPrefab;
    [SerializeField] private GameObject LootUI;
    [SerializeField] private GameObject PlayerInventory;
    public LootPool lootPool; 
    public float openDuration = 1000f; // 1000 = 1 second
    public Slider lootProgressSlider; 
    private float openProgress = 0f;
    private bool isLooting = false;
    public bool inrange;
    public bool itemdropped = false;
    private UIcontrollerExpidition uIcontrollerExpidition;

    public KeyCode lootKey = KeyCode.F;
    private InventoryItemPresent inventoryItemPresent;
    private ExpenditionManager expenditionManager;

    private SpriteRenderer spriteRenderer;

    // List of items currently dropped and awaiting player action:
    public List<ItemData> droppedItems = new List<ItemData>();

    // Tracks if we've already shown loot UI once
    private bool lootUIOpened = false;

    void Start()
    {
        uIcontrollerExpidition = FindObjectOfType<UIcontrollerExpidition>();
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        lootProgressSlider.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isLooting && !itemdropped)
        {
            if (Input.GetKey(lootKey))
            {
                openProgress += (100f / (openDuration / 1000f)) * Time.deltaTime;
                openProgress = Mathf.Clamp(openProgress, 0f, 100f);
                UpdateLootProgressUI(openProgress);

                if (openProgress >= 100f)
                {
                    GiveLoot();
                    ResetLooting();
                }
            }
            else if (Input.GetKeyUp(lootKey))
            {
                ResetLooting();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = true;

            // If loot hasn't been opened yet (no dropped items)
            // turn green when in range and not looted yet:
            if (droppedItems.Count == 0 && !lootUIOpened)
            {
                spriteRenderer.DOColor(Color.green, 0.5f);
            }
            if (!isLooting && (Input.GetKey(lootKey) || Input.GetKeyDown(KeyCode.Tab)))
            {
                if (droppedItems.Count > 0)
                {
                    // Show existing loot
                    OpenLootUI();
                }
                else
                {
                    isLooting = true;
                    openProgress = 0f;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = false;

            if (droppedItems.Count == 0)
            {
                spriteRenderer.DOColor(Color.white, 0.5f);
            }
            else if (droppedItems.Count >= 1 && lootUIOpened)
            {
                spriteRenderer.DOColor(Color.yellow, 0.5f);
            }
        }
    }

    private void ResetLooting()
    {
        isLooting = false;
        openProgress = 0f;
        UpdateLootProgressUI(openProgress);
        lootProgressSlider.gameObject.SetActive(false);
    }

    private void GiveLoot()
    {
        if (lootPool == null || lootPool.lootItems.Count == 0) return;

        var lootResult = lootPool.GetRandomLoot();
        if (lootResult.item != null && lootResult.amount > 0)
        {
            ItemData newItemData = new ItemData
            {
                nameItem = lootResult.item.nameItem,
                idItem = lootResult.item.idItem,
                count = lootResult.amount,
                maxCount = lootResult.item.maxCount,
                itemtype = lootResult.item.itemtype
            };

            droppedItems.Add(newItemData);
            inventoryItemPresent.RefreshUIBox();

            itemdropped = true;
            lootUIOpened = true;
            OpenLootUI();
        }
    }

    private void UpdateLootProgressUI(float progress)
    {
        if (lootProgressSlider != null)
        {
            lootProgressSlider.gameObject.SetActive(true);
            lootProgressSlider.value = progress / 100f;
        }
    }

    public void OpenLootUI()
    {
        // Clear any existing UI elements
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        PlayerInventory.SetActive(true);
        uIcontrollerExpidition.ToggleInventoryUI();
        LootUI.SetActive(true);

        foreach (var item in droppedItems)
        {
            UIItemData prefabData = inventoryItemPresent.listUIItemPrefab.FirstOrDefault(prefab => prefab.idItem == item.idItem);

            if (prefabData != null)
            {
                // Instantiate the UI item and get the necessary components
                GameObject newItemUI = Instantiate(prefabData.gameObject, itemsContainer);
                UIItemData uiItemData = newItemUI.GetComponent<UIItemData>();
                uiItemData.originatingLootSystem = this;

                ItemClass itemClass = newItemUI.GetComponent<ItemClass>();

                // Set up the itemClass properties from the item data
                itemClass.nameItem = item.nameItem;
                itemClass.idItem = item.idItem;
                itemClass.quantityItem = item.count;
                itemClass.maxCountItem = item.maxCount;
                itemClass.itemtype = item.itemtype;

                // Set up the UIItemData properties
                uiItemData.idItem = item.idItem;
                uiItemData.nameItem = item.nameItem;
                uiItemData.slotTypeParent = SlotType.SlotBoxes;

                // Assign the item icon if available
                Sprite icon = inventoryItemPresent.GetItemIconByID(item.idItem);
                if (icon != null)
                {
                    uiItemData.itemIconImage.sprite = icon;
                }

                // Update the UI with the new item data
                uiItemData.UpdateDataUI(itemClass);
            }
            else
            {
                Debug.LogWarning("No matching UI prefab found for item ID: " + item.idItem);
            }
        }

        gameObject.SetActive(true);
    }


    public void CloseLootUI(bool grabbedAll)
    {
        OnLootUIClosed(grabbedAll);

        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
    }

    public void CollectAllItems()
    {
        foreach (var item in droppedItems)
        {
            expenditionManager.AddItemToInventorySlot(item);
        }

        droppedItems.Clear();
    }

    public void OnLootUIClosed(bool grabbedAll)
    {
        if (grabbedAll)
        {
            if (droppedItems.Count == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (droppedItems.Count > 0)
            {
                spriteRenderer.DOColor(Color.yellow, 0.5f);
            }
        }

        LootUI.SetActive(false);
        uIcontrollerExpidition.ToggleInventoryUI();
        PlayerInventory.SetActive(false);
    }
}
