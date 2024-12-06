using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontrollerExpidition : MonoBehaviour
{
    public GameObject InventoryUI;
    public ActionController actionController;
    private bool isInventoryActive = false;
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

    }
    void ToggleUI(bool inventoryActive)
    {
        if (InventoryUI != null) InventoryUI.SetActive(inventoryActive);
    }
    void ToggleInventoryUI()
    {
        isInventoryActive = !isInventoryActive;
        if (isInventoryActive)
        {
            actionController.canuseweapon = false;
            ToggleUI(true);
        }
        else
        {
            actionController.canuseweapon = true;
            ToggleUI(false);
        }
    }
}
