using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanger : MonoBehaviour
{
    public GameObject UpgradeUI;
    public GameObject WorkshopUI;
    public GameObject workshopUpgradeUI;
    public GameObject ExpiditionUI;
    private bool isExpiditionUIActive; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToogleExpiditionUI();
        }
    }
    public void ToogleExpiditionUI()
    {
        isExpiditionUIActive = !isExpiditionUIActive;
        ExpiditionUI.SetActive(isExpiditionUIActive);
    }
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
    public void DisableUpgradeworkshopButton()
    {
        workshopUpgradeUI.SetActive(false);
    }
}
