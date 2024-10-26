using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBP : UIInventory
{

    public void Start()
    {
        SetValuableUIInventory();
        RefreshUIInventory();
    }
    private void OnEnable()
    {
        RefreshUIInventory();
    }
    private void OnDisable()
    {
        ConventDataUIToItemData();
    }
}
