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
    private LootingSystem currentLootSystem;
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
                    // At this point, the loot UI should be shown to the player, 
                    // do not disable the game object yet.
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

            // If player presses loot key and loot isn't currently being opened:
            if (!isLooting && Input.GetKey(lootKey))
            {
                // If items are already dropped (previously opened),
                // just show loot UI again without progress:
                if (droppedItems.Count > 0)
                {
                    // Show existing loot
                    OpenLootUI(this);
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

            // Revert color when out of range, unless partially looted:
            if (droppedItems.Count == 0)
            {
                spriteRenderer.DOColor(Color.white, 0.5f);
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
            // Instead of directly adding to inventory, store them in droppedItems
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
            OpenLootUI(this);
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

    // Called after loot is generated or when the player re-interacts with a partially looted container:
    public void OpenLootUI(LootingSystem lootSystem)
    {
        currentLootSystem = lootSystem;

        // Clear any existing UI elements
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
        PlayerInventory.SetActive(true);
        uIcontrollerExpidition.ToggleInventoryUI();
        LootUI.SetActive(true);
        List<ItemData> droppedItems = currentLootSystem.GetDroppedItems(); 
        foreach (var item in droppedItems)
        {
            UIItemData prefabData = inventoryItemPresent.listUIItemPrefab
            .FirstOrDefault(prefab => prefab.idItem == item.idItem);

            if (prefabData != null)
            {
                // Instantiate the UI element for this item
                GameObject itemUI = Instantiate(prefabData.gameObject, itemsContainer);

                // Get the UIItemData and ItemClass components from the instantiated object
                UIItemData uIItemData = itemUI.GetComponent<UIItemData>();
                ItemClass itemClass = itemUI.GetComponent<ItemClass>();

                // Set itemClass fields based on the ItemData 'item'
                itemClass.nameItem = item.nameItem;
                itemClass.idItem = item.idItem;
                itemClass.quantityItem = item.count;
                itemClass.maxCountItem = item.maxCount;
                itemClass.itemtype = item.itemtype;

                // Set UIItemData fields
                uIItemData.idItem = item.idItem;
                uIItemData.nameItem = item.nameItem;
                // Set the parent slot type if you know it, for example:
                uIItemData.slotTypeParent = SlotType.SlotBoxes;

                // Get the icon from InventoryItemPresent
                Sprite icon = inventoryItemPresent.GetItemIconByID(item.idItem);
                if (icon != null)
                {
                    // itemClass.IconSprite is an Image, so let's link it to uIItemData.itemIconImage
                    itemClass.IconSprite = uIItemData.itemIconImage;
                    itemClass.IconSprite.sprite = icon;
                }

                // Update the UI to reflect this item's data
                uIItemData.UpdateDataUI(itemClass);
            }
            else
            {
                Debug.LogWarning("No matching UI prefab found for item ID: " + item.idItem);
            }
        }

        // Show the UI panel
        gameObject.SetActive(true);
    }

    // This method should be called by your UI once the player is done interacting:
    // grabbedAll: true if player took all items, false if left some behind.
    public void CloseLootUI(bool grabbedAll)
    {
        if (currentLootSystem != null)
        {
            // Notify the LootingSystem that the UI is closing
            currentLootSystem.OnLootUIClosed(grabbedAll);
        }

        // Clear the UI container
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        // Hide the UI
        gameObject.SetActive(false);
        currentLootSystem = null;
    }
    
    public List<ItemData> GetDroppedItems()
    {
        // Return the droppedItems list (assuming droppedItems is a List<ItemData>)
        return droppedItems;
    }

    // Called when the player chooses to grab all items from the UI.
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
            // Player took all items
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
