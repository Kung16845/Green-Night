using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClinicUI : MonoBehaviour
{
    private Clinic clinic;
    public TextMeshProUGUI AviableBed;
    public TextMeshProUGUI UpgradeBenefits;
    public TextMeshProUGUI HealingLate;
    public Transform DisplayParent;    // For displaying patients currently being healed or injured NPCs
    public Transform SelectParent;     // For displaying select slots or buttons
    public GameObject SelectPrefab;
    public GameObject PatienPrefab;
    public UImanger uImanger;
    private PatienManger patienManger;
    private NpcManager npcManager;

    // Keep a reference of injured NPCs currently displayed for selection
    private List<NpcClass> displayedInjuredNpcs = new List<NpcClass>();

    void OnEnable()
    {
        uImanger = FindObjectOfType<UImanger>();
        npcManager = FindObjectOfType<NpcManager>();
        patienManger = FindObjectOfType<PatienManger>();
        clinic = FindObjectOfType<Clinic>();
        UpdateUI();
    }

    public void InitializeUpgradeData()
    {
        clinic.AssignUpgradeData();
    }

    public void displayPatient()
    {
        // Clear existing displays
        foreach (Transform child in DisplayParent)
            Destroy(child.gameObject);
        foreach (Transform child in SelectParent)
            Destroy(child.gameObject);

        int totalSlots = 2; 
        int displayedCount = 0;

        // Display currently healing patients in the clinic
         foreach (var patient in patienManger.activeHealingClinicPatient)
        {
            if (displayedCount >= totalSlots) break;

            // Instantiate a select slot for this patient

            // Instantiate the patient prefab inside the display parent
            GameObject patientObj = Instantiate(PatienPrefab, SelectParent);

            // Retrieve NPC data from npcManager (looking in working list first, then normal list)
            NpcClass npcData = npcManager.listNpcWorking.FirstOrDefault(npc => npc.idnpc == patient.NpcID);
            if (npcData == null)
            {
                npcData = npcManager.listNpc.FirstOrDefault(npc => npc.idnpc == patient.NpcID);
            }

            if (npcData != null)
            {
                // Get face image
                Sprite faceSprite = GetNpcFaceSprite(npcData);

                // Set data on the patient UI
                PatientUIItem uiItem = patientObj.GetComponent<PatientUIItem>();
                if (uiItem != null)
                {
                    uiItem.SetData(npcData.nameNpc, faceSprite, patient.Npchp);
                }
            }

            displayedCount++;
        }

        int remainingSlots = totalSlots - displayedCount;
        for (int i = 0; i < remainingSlots; i++)
        {
            Debug.Log(remainingSlots);
            // Create an empty slot (no patient object needed here)
            GameObject slotObj = Instantiate(SelectPrefab, SelectParent);
            SelectButtonItem selectButtonItem = slotObj.GetComponent<SelectButtonItem>();
            if (selectButtonItem != null)
            {
                // For empty slots, clicking the button should display injured NPCs
                selectButtonItem.InitializeForEmptySlot(this);
            }
        }
    }


    public void DisplayInjuredNpc()
    {
        uImanger.ToggleUIPanel(UImanger.UIPanel.ClinicInhuredNpcUI);
        foreach (Transform child in DisplayParent)
            Destroy(child.gameObject);
        foreach (Transform child in SelectParent)
            Destroy(child.gameObject);

        displayedInjuredNpcs.Clear();

        // Display only NPCs that are injured (hp < 100) from listNpc
        var injuredNpcs = npcManager.listNpc.Where(npc => npc.hp < 100).ToList();
        foreach (var npcData in injuredNpcs)
        {
            // Instantiate patient prefab
            GameObject patientObj = Instantiate(PatienPrefab, DisplayParent);

            // Get face image
            Sprite faceSprite = GetNpcFaceSprite(npcData);

            // Set data on the patient UI
            PatientUIItem uiItem = patientObj.GetComponent<PatientUIItem>();
            if (uiItem != null)
            {
                uiItem.SetData(npcData.nameNpc, faceSprite, npcData.hp);
                uiItem.InitializeButton(this, npcData.idnpc); // Button on PatienPrefab
            }
            displayedInjuredNpcs.Add(npcData);
        }
    }

    // Updated AddPatient function to take an npcId
    public void AddPatient(int npcId)
    {
        // Find the NPC with this ID in the displayedInjuredNpcs
        NpcClass npcToAdd = displayedInjuredNpcs.FirstOrDefault(n => n.idnpc == npcId);
        if (npcToAdd != null)
        {
            float healingRate = 2f; // example healing rate
            patienManger.AddPatient(npcToAdd.idnpc, npcToAdd.hp, healingRate, PatienSourceSource.Clinic);
            npcManager.MoveNpcToWorking(npcToAdd.idnpc);

            Debug.Log($"Added NPC {npcToAdd.nameNpc}(ID: {npcToAdd.idnpc}) as a patient to the clinic.");

            // Refresh UI
            DisplayInjuredNpc();
            displayPatient();
        }
        else
        {
            Debug.LogWarning($"No injured NPC with ID {npcId} found in the currently displayed list.");
        }
    }

    void UpdateUI()
    {
        if (clinic != null)
        {
            int bed = clinic.CurrentActiveCurebed;
            AviableBed.text = $"Bed Count: {bed} ";

            if (clinic.upgradeBuilding.currentLevel < clinic.upgradeBuilding.maxLevel)
            {
                UpgradeBenefits.text = $"Upgrade Benefit: Provide 2 Bed.";
            }
            else
            {
                UpgradeBenefits.text = "Fully Upgraded";
            }
        }
        else
        {
            AviableBed.text = "clinic not found";
            UpgradeBenefits.text = string.Empty;
        }
    }

    private Sprite GetNpcFaceSprite(NpcClass npcData)
    {
        // Retrieve the head sprite based on npcData.idHead
        HeadCoutume headCoutume = npcManager.listHeadCoutume.FirstOrDefault(c => c.idHead == npcData.idHead);
        if (headCoutume != null)
        {
            return headCoutume.spriteHead;
        }
        return null;
    }
}
