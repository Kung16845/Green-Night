using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{   
    private string[] firstNames = { "John", "Jane", "Alex", "Emily", "Chris", "Sara" };
    private string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia" };
    [Header("List Coutume")]
    public List<HeadCoutume> listHeadCoutume = new List<HeadCoutume>();
    public List<BodyCoutume> listBodyCoutume = new List<BodyCoutume>();
    public List<FeedCoutume> listFeedCoutume = new List<FeedCoutume>();
    [Header("Npc")]
    public List<NpcClass> listNpc = new List<NpcClass>();
    public List<NpcClass> listNpcWorkingWIthInOneDay = new List<NpcClass>();
    public List<NpcClass> listNpcWorkingMoreOneDay = new List<NpcClass>();
    public TMP_Dropdown dropdown;
    public InventoryItemPresent inventoryItemPresent;
    private void Start()
    {   
        inventoryItemPresent = FindObjectOfType<InventoryItemPresent>();

        dropdown = FindObjectOfType<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        
        StartGameCreateGropNpx();
        SetOptionDropDown();
        OnDropdownValueChanged(0);
    }
    public void SetOptionDropDown()
    {   
        dropdown.ClearOptions();

        List<string> newOption = new List<string>();

        foreach (NpcClass npcData in listNpc)
        {
            newOption.Add(npcData.roleNpc.ToString());
        }

        dropdown.AddOptions(newOption); 
    }
    public void StartGameCreateGropNpx()
    {

        for (int i = 0; i < 5;i++)
        {
            CreateNpc(i);
        }

    }
    public void CreateNpc(int idNpc)
    {   

        NpcClass newNpc = new NpcClass();

        string randomFirstName = firstNames[Random.Range(0, firstNames.Length)];
        string randomLastName = lastNames[Random.Range(0, lastNames.Length)];

        // newNpc.name = randomFirstName + " " + randomLastName;   
        newNpc.roleNpc = (SpecialistRoleNpc)Random.Range(0,8);

        newNpc.countInventorySlot = Random.Range(6,13);

        newNpc.idnpc = idNpc;

        newNpc.idHead = Random.Range(0, listHeadCoutume.Count);
        newNpc.idBody = Random.Range(0,listBodyCoutume.Count);
        newNpc.idFeed = Random.Range(0,listFeedCoutume.Count);

        listNpc.Add(newNpc);
    }
    public void OnDropdownValueChanged(int selectedValue)
    {
        Debug.Log("Dropdown index changed to: " + selectedValue);
        Debug.Log("Dropdown Option changed to: " + dropdown.options[selectedValue].text);
        NpcClass npcClassSelest = listNpc.FirstOrDefault(npc => npc.idnpc == selectedValue);

        inventoryItemPresent.UnlockSlotInventory(npcClassSelest.countInventorySlot,npcClassSelest.roleNpc);

        HandleSpecialistNpcChange(selectedValue);
    }
    public void HandleSpecialistNpcChange(int numSpecialistNpc)
    {
        SpecialistRoleNpc specialistNpc = (SpecialistRoleNpc)numSpecialistNpc;

        switch (specialistNpc)
        {
            case SpecialistRoleNpc.Handicraft:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Maintainance:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Network:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Scavenger:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Military_training:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Chemical:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Doctor:
                SetSpecialistNpc(specialistNpc);
                break;
            case SpecialistRoleNpc.Entertainer:
                SetSpecialistNpc(specialistNpc);
                break;
            default:
                Debug.LogWarning("Invalid SpecialistNpc selected");
                break;
        }
    }
    public void SetSpecialistNpc(SpecialistRoleNpc specialistNpc)
    {   

        Debug.Log("Specialist Player : " + specialistNpc);
        
    }

}
