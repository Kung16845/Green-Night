using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootingSystem : MonoBehaviour
{
    public LootPool lootPool; // Reference to the loot pool
    public float openDuration = 1000f; // 1000 = 1 second
    private float openProgress = 0f;
    private bool isLooting = false;
    public bool inrange;
    public bool itemdropped;

    public KeyCode lootKey = KeyCode.F;
    private InventoryItemPresent inventoryItemPresent;

    void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
    }

    void Update()
    {
        if (isLooting)
        {
            // Continue the looting process while the player holds the loot key
            if (Input.GetKey(lootKey))
            {
                openProgress += (100f / (openDuration / 1000f)) * Time.deltaTime;
                UpdateLootProgressUI(openProgress);

                if (openProgress >= 100f)
                {
                    GiveLoot();
                    ResetLooting();
                }
            }
            // Cancel looting if the player releases the loot key
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
            if (!isLooting && Input.GetKey(lootKey))
            {
                isLooting = true;
                openProgress = 0f;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        inrange = false;
    }

    private void ResetLooting()
    {
        isLooting = false;
        openProgress = 0f;
        UpdateLootProgressUI(openProgress);
    }

    private void GiveLoot()
    {
        if (lootPool == null || lootPool.lootItems.Count == 0) return;

        // Use the loot pool to get a random item
        ItemData itemToLoot = lootPool.GetRandomItem();
        inventoryItemPresent.AddItem(itemToLoot);
        inventoryItemPresent.RefreshUI();
        itemdropped = true;
    }

    private void UpdateLootProgressUI(float progress)
    {
        // Example: Update a progress bar in the UI
        Debug.Log("Looting Progress: " + progress);
    }
}
