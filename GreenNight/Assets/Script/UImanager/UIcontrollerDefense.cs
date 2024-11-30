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
    public ActionController actionController;
    private bool isPlayerNear = false;
    private bool isInventoryActive = false;
    private bool isBoxActive = false;

    void Start()
    {
        actionController.canuseweapon = false;
    }
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

    void ToggleUI(bool mainboxActive,bool inventoryActive, bool barrierHPActive, bool playerActive, bool boxItemActive)
    {
        if (MainBox != null) MainBox.SetActive(mainboxActive);
        if (InventoryUI != null) InventoryUI.SetActive(inventoryActive);
        if (BarrierHPUI != null) BarrierHPUI.SetActive(barrierHPActive);
        if (PlayerUI != null) PlayerUI.SetActive(playerActive);
        if (BoxItemUI != null) BoxItemUI.SetActive(boxItemActive);
    }

    void ToggleInventoryUI()
    {
        isInventoryActive = !isInventoryActive;
        if (isInventoryActive)
        {
            actionController.canuseweapon = false;
            ToggleUI(true, true, false, false, false);
        }
        else
        {
            actionController.canuseweapon = true;
            ToggleUI(false, false, true, true, false);
        }
    }

    void ToggleBoxUI()
    {
        isBoxActive = !isBoxActive;

        if (isBoxActive)
        {
            actionController.canuseweapon = false;
            ActiveBoxUI();
        }
        else
        {
            actionController.canuseweapon = true;
            DisableBoxUI();
        }
    }

    void ActiveBoxUI()
    {
        ToggleUI(true, true, false, false, true);
    }

    void DisableBoxUI()
    {
        ToggleUI(false, false, true, true, false);
    }
}
