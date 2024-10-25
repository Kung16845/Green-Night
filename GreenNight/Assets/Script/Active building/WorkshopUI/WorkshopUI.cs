using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopUI : MonoBehaviour
{
    public Workshop workshop;
    public TextMeshProUGUI Craftingslot;
    public TextMeshProUGUI ActionSpeed;
    void Start()
    {
        workshop = FindObjectOfType<Workshop>();
        Assignactionspeedandslot();
    }
    void Assignactionspeedandslot()
    {
        float actionSpeedIncreasePercent = workshop.Actionspeedincrease * 100f;
        ActionSpeed.text = "Action Speed: +" + actionSpeedIncreasePercent.ToString("F0") + "%";
        int Slotincrease = workshop.Craftingslot;
        Craftingslot.text = "Crafting slot: +" + Slotincrease.ToString("F0");
    }
    void Update()
    {
          if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);  // Disable the GameObject this script is attached to
        }
    }
}
