using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanger : MonoBehaviour
{
    public GameObject UpgradeUI;
    public void ActiveUpgradeUI()
    {
        UpgradeUI.SetActive(true);
    }
}
