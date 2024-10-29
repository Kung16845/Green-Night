using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryEX : UIInventory
{

    private void Awake()
    {
        SetValuableUIInventory();
    }
    private void Start()
    {
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
