using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontrollerDefense : MonoBehaviour
{
    public GameObject InventoryUI;
    public GameObject BoxItemUI;
    public GameObject BarrierHPUI;
    public GameObject PlayerUI;
    public GameObject MainBox;

    private bool isPlayerNear = false;
    private bool isInventoryActive = false;
    private bool isBoxActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleBoxUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (isBoxActive)
            {
                DisableBoxUI(); // Automatically disable the box UI when the player leaves the range
            }
        }
    }

    void ToggleUI(bool inventoryActive, bool barrierHPActive, bool playerActive, bool boxItemActive = false, bool mainboxActive = false)
    {
        if (InventoryUI != null) InventoryUI.SetActive(inventoryActive);
        if (BarrierHPUI != null) BarrierHPUI.SetActive(barrierHPActive);
        if (PlayerUI != null) PlayerUI.SetActive(playerActive);
        if (BoxItemUI != null) BoxItemUI.SetActive(boxItemActive);
        if (MainBox != null) MainBox.SetActive(mainboxActive);
    }

    void ToggleInventoryUI()
    {
        isInventoryActive = !isInventoryActive;

        if (isInventoryActive)
        {
            ToggleUI(true, false, false, false, true);
        }
        else
        {
            ToggleUI(false, true, true, false, false);
        }
    }

    void ToggleBoxUI()
    {
        isBoxActive = !isBoxActive;

        if (isBoxActive)
        {
            ActiveBoxUI();
        }
        else
        {
            DisableBoxUI();
        }
    }

    void ActiveBoxUI()
    {
        ToggleUI(true, false, false, true, true);
    }

    void DisableBoxUI()
    {
        ToggleUI(false, true, true, false, false);
    }
}
