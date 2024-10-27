using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanger : MonoBehaviour
{
    public GameObject UpgradeUI;
    public GameObject WorkshopUI;
    public void ActiveUpgradeUI()
    {
        UpgradeUI.SetActive(true);
    }
    public void DisableUpgradeUI()
    {
        UpgradeUI.SetActive(false);
    }
    public void ActiveWorkshopUI()
    {
        WorkshopUI.SetActive(true);
    }
    public void DisableWorkshopUI()
    {
        WorkshopUI.SetActive(false);
    }
}
