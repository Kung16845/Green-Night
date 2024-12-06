using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using DG.Tweening; // Ensure DOTween is imported

public class LootingSystem : MonoBehaviour
{
    public LootPool lootPool; // Reference to the loot pool
    public float openDuration = 1000f; // 1000 = 1 second
    public Slider lootProgressSlider; 
    private float openProgress = 0f;
    private bool isLooting = false;
    public bool inrange;
    public bool itemdropped = false;

    public KeyCode lootKey = KeyCode.F;
    private InventoryItemPresent inventoryItemPresent;
    private ExpenditionManager expenditionManager;

    private SpriteRenderer spriteRenderer; // Reference to the object's sprite renderer

    void Start()
    {
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        lootProgressSlider.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>(); // Ensure the object has a SpriteRenderer
    }

    void Update()
    {
        if (isLooting && !itemdropped)
        {
            // Continue the looting process while the player holds the loot key
            if (Input.GetKey(lootKey))
            {
                openProgress += (100f / (openDuration / 1000f)) * Time.deltaTime;
                openProgress = Mathf.Clamp(openProgress, 0f, 100f); // Ensure progress stays between 0 and 100
                UpdateLootProgressUI(openProgress);

                if (openProgress >= 100f)
                {
                    GiveLoot();
                    ResetLooting(); // Reset looting state after giving loot
                    lootProgressSlider.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
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

            // Turn green when in range
            spriteRenderer.DOColor(Color.green, 0.5f);

            if (!isLooting && Input.GetKey(lootKey))
            {
                isLooting = true;
                openProgress = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = false;

            // Revert color when out of range
            spriteRenderer.DOColor(Color.white, 0.5f);
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

        // Use the loot pool to get a random item and amount
        var lootResult = lootPool.GetRandomLoot();

        if (lootResult.item != null && lootResult.amount > 0)
        {
            // Add the chosen quantity of this item to the inventory
            for (int i = 0; i < lootResult.amount; i++)
            {
                expenditionManager.AddItemToInventorySlot(lootResult.item);
            }
            inventoryItemPresent.RefreshUIBox();
            itemdropped = true;
        }
    }

    private void UpdateLootProgressUI(float progress)
    {
        if (lootProgressSlider != null)
        {
            lootProgressSlider.gameObject.SetActive(true);
            lootProgressSlider.value = progress / 100f; // Update slider value (normalized between 0 and 1)
        }
    }
}
