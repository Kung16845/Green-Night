using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Sattlelite : MonoBehaviour
{
    public bool SatelliteOnline = false;
    public bool RecondroneActive = false;
    // public bool SuuplyDrop;
    public TimeManager timeManager;
    public DateTime dateTime;
    public int currentDay;
    public UImanger uImanger;
    public BuildManager buildManager;
    public Globalstat globalstat;
    public InventoryItemPresent inventoryItemPresent;
    public SpriteRenderer spriteRenderer;
    public Sprite repairedSpriteRenderer;
    private int daycost = 2;
    public bool isRepairing = false;
    private int npcCost = 1;
    public int finishDayBuildingTime = 0;
    public NpcManager npcManager;
    public TextMeshProUGUI SattleliteStatusText;
    public TextMeshProUGUI SattleliteWire;
    public TextMeshProUGUI SattleliteCircuit;
    public TextMeshProUGUI SattleliteSteel; // Combined status and hint
    public Image newworkIcon;
    public Button RepariButton;
    void Start()
    {
        dateTime = timeManager.dateTime;
    }

    void OnMouseDown()
    {
        uImanger.ToggleUIPanel(UImanger.UIPanel.SattleliteUI);

        UpdateSattleliteStatusText();
        UpdateRepairButton();
        UpdateNewWorkIcon(SpecialistRoleNpc.Network);
    }
    void Update()
    {
        WaitRepair();
        CheckMaintenance();
    }
    private void UpdateNewWorkIcon(SpecialistRoleNpc requiredSpecialist)
    {
        // Check if the required specialist exists
        bool hasSpecialist = HasRequiredSpecialist(requiredSpecialist);

        if (newworkIcon != null)
        {
            newworkIcon.gameObject.SetActive(hasSpecialist); // Show or hide the icon
        }
    }
    public void WaitRepair()
    {
        if (dateTime.day >= finishDayBuildingTime && isRepairing)
        {
            isRepairing = false;
            SatelliteOnline = true;
            globalstat.SatelliteOnline = SatelliteOnline;
            buildManager.npc += npcCost;

            // Change the sprite to the repaired version
            if (spriteRenderer != null && repairedSpriteRenderer != null)
            {
                spriteRenderer.sprite = repairedSpriteRenderer;
            }

            return;
        }
    }

    void CheckMaintenance()
    {
        if (SatelliteOnline&& !buildManager.iselecticitiesactive)
        {
            SatelliteOnline= false;
            globalstat.SatelliteOnline = false;
        }
        else if (SatelliteOnline)
        {
            SatelliteOnline= true;
            globalstat.SatelliteOnline = true;
        }
    }
    public void InitializeRepair()
    {
        isRepairing = true;
        ItemData itemDataToRemove = new ItemData
        {
            idItem = 1020102,
            count = 20
        };
        inventoryItemPresent.RemoveItem(itemDataToRemove);
        ItemData itemDataToRemove2 = new ItemData
        {
            idItem = 1020103,
            count = 30
        };
        inventoryItemPresent.RemoveItem(itemDataToRemove2);
        dateTime = timeManager.dateTime;
        buildManager.steel -= 3;
        finishDayBuildingTime += dateTime.day + daycost;
        buildManager.npc -= npcCost;
        AssignSpecialistToUpgrade(SpecialistRoleNpc.Network);
        uImanger.DisableUIPanel(UImanger.UIPanel.SattleliteUpgradeButton);
        uImanger.DisableUIPanel(UImanger.UIPanel.SattleliteUI);
    }
    private void UpdateSattleliteStatusText()
    {
        int currentCircuit = inventoryItemPresent.GetItemCountByID(1020102);
        int requiredCircuit = 20;
        int currentWire = inventoryItemPresent.GetItemCountByID(1020103);
        int requiredWire = 30;
        int currentSteel = buildManager.steel;
        int requiredSteel = 3;

        // Update individual material UI
        SattleliteCircuit.text = $"Circuits: <color=yellow>{currentCircuit}/{requiredCircuit}</color>";
        SattleliteWire.text = $"Wires: <color=yellow>{currentWire}/{requiredWire}</color>";
        SattleliteSteel.text = $"Steel: <color=yellow>{currentSteel}/{requiredSteel}</color>";

        // Update the main satellite status text
        if (!SatelliteOnline && !isRepairing)
        {
            SattleliteStatusText.text =
                "The satellite is damaged. Once repaired, it can be used to call an airstrike during the night.";
        }
        else if (isRepairing && !buildManager.iselecticitiesactive)
        {
            SattleliteStatusText.text =
                "The satellite is currently being repaired, but there’s no power supply. Even if repaired, it cannot be used without electricity.";
        }
        else if (isRepairing && buildManager.iselecticitiesactive)
        {
            SattleliteStatusText.text =
                "The satellite is being repaired and we have power. It should be operational soon.";
        }
        else if (SatelliteOnline && !buildManager.iselecticitiesactive)
        {
            SattleliteStatusText.text =
                "The satellite is online, but there’s no electricity to power it. Restore power to use its functionality.";
        }
        else if (SatelliteOnline && buildManager.iselecticitiesactive)
        {
            SattleliteStatusText.text =
                "The satellite is fully operational and ready for use.";
        }
        else
        {
            SattleliteStatusText.text = string.Empty; // Fallback case
        }
    }
    private void UpdateRepairButton()
    {
        int currentCircuit = inventoryItemPresent.GetItemCountByID(1020102);
        int requiredCircuit = 20;
        int currentWire = inventoryItemPresent.GetItemCountByID(1020103);
        int requiredWire = 30;
        int currentSteel = buildManager.steel;
        int requiredSteel = 3;

        // Check if sufficient materials are available
        bool hasSufficientMaterials = currentCircuit >= requiredCircuit &&
                                    currentWire >= requiredWire &&
                                    currentSteel >= requiredSteel;

        // Check if a specialist is available
        bool hasRequiredSpecialist = HasRequiredSpecialist(SpecialistRoleNpc.Network);

        // Update button interactability
        RepariButton.interactable = hasSufficientMaterials && hasRequiredSpecialist;
    }


    private void AssignSpecialistToUpgrade(SpecialistRoleNpc requiredSpecialist)
    {
        // Find the NPC with the required specialist role
        NpcClass specialistNpc = npcManager.listNpc.Find(npc => npc.roleNpc == requiredSpecialist);

        if (specialistNpc != null)
        {
            // Remove the NPC from the available list and add to working list
            npcManager.listNpc.Remove(specialistNpc);
            npcManager.listNpcWorking.Add(specialistNpc);
        }
    }
    private bool HasRequiredSpecialist(SpecialistRoleNpc requiredSpecialist)
    {
        // Check if there is at least one NPC with the required specialist role
        return npcManager.listNpc.Exists(npc => npc.roleNpc == requiredSpecialist);
    }

}
// <color=#FFFF00>Dynamite collected: {currentsteel}/{RequieSteel}</color>
public enum SuuplyDropType
{
    FirePower,
    Chemical,
    Food,
    Building
}