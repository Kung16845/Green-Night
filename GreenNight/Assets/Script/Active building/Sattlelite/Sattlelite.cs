using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Sattlelite : MonoBehaviour
{
    public bool Sattleliteonline;
    public TimeManager timeManager;
    public DateTime dateTime;
    public int currentDay;
    public UImanger uImanger;
    public BuildManager buildManager;
    public Globalstat globalstat;
    public InventoryItemPresent inventoryItemPresent;
    public SpriteRenderer spriteRenderer;
    private int daycost = 2;
    public bool isRepairing;
    private int npcCost = 1;
    public int finishDayBuildingTime = 0;
    public NpcManager npcManager;
    public TextMeshProUGUI SattleliteStatusText; // Combined status and hint
    void Start()
    {
        dateTime = timeManager.dateTime;
    }

    void OnMouseDown()
    {
        uImanger.ToggleUIPanel(UImanger.UIPanel.TunnelUI);

        UpdateStallelitestatusText();
    }
    void Update()
    {
        WaitRepair();
        CheckMaintenance();
    }
    public void WaitRepair()
    {
        if (dateTime.day >= finishDayBuildingTime &&  isRepairing)
        {
            isRepairing = false;
            Sattleliteonline = true;
            globalstat.sattleliteonline = Sattleliteonline;
            buildManager.npc += npcCost;
            return;
        }
    }
    void CheckMaintenance()
    {
        if (Sattleliteonline&& !buildManager.iswateractive)
        {
            spriteRenderer.enabled = false;
            Sattleliteonline= false;
            globalstat.Tunnelaviable = false;
        }
        else if (Sattleliteonline)
        {
            spriteRenderer.enabled = false;
            Sattleliteonline= true;
            globalstat.Tunnelaviable = true;
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
        finishDayBuildingTime += dateTime.day + daycost;
        buildManager.npc -= npcCost;
        uImanger.DisableUIPanel(UImanger.UIPanel.ClearingTunnelUI);
        uImanger.DisableUIPanel(UImanger.UIPanel.TunnelUI);
    }
    private void UpdateStallelitestatusText()
    {
        if (!Sattleliteonline && !isRepairing)
        {
            int currentCircuit = inventoryItemPresent.GetItemCountByID(1020102);
            int requiredCircuit = 20;
            int currentWire = inventoryItemPresent.GetItemCountByID(1020103);
            int requiredWire = 30;
            int currentsteel = buildManager.steel;
            int RequieSteel = 3;
            
            SattleliteStatusText.text = $"The tunnel is blocked. If we clear it, we might find something useful. Rumor has it the military left supplies here. <color=#FFFF00>Dynamite collected: {currentsteel}/{RequieSteel}</color>";
        }
        else if (isRepairing && !buildManager.iswateractive)
        {
            SattleliteStatusText.text = "The tunnel is being cleared, but it's flooded. This might take longer than expected.";
        }
        else if (isRepairing && buildManager.iswateractive)
        {
            SattleliteStatusText.text = "The tunnel is being cleared, and the water is being drained.";
        }
        else if (Sattleliteonline && !buildManager.iswateractive)
        {
            SattleliteStatusText.text = "The tunnel is open, but it's still flooded with water. We can't explore for supplies yet.";
        }
        else if (Sattleliteonline && buildManager.iswateractive)
        {
            SattleliteStatusText.text = "The tunnel is now open and clear! It's a safe route for expeditions and supplies are accessible.";
        }
        else
        {
            SattleliteStatusText.text = string.Empty;
        }
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
