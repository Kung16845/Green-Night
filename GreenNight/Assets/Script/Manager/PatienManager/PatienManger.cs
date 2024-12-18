using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienManger : MonoBehaviour
{
    public List<CurePatient> activeHealingClinicPatient = new List<CurePatient>();
    public List<CurePatient> activeHealingHospitalPatient = new List<CurePatient>();

    // Reference to the NpcManager (can be assigned in the inspector or found at runtime)
    public NpcManager npcManager;
    public void AddPatient(int npcId, float currentHp, float healingRate, PatienSourceSource source)
    {
        // Create a new CurePatient object
        CurePatient newPatient = new CurePatient(npcId, currentHp, healingRate, source);

        // Decide which list to add the patient to based on the source
        switch (source)
        {
            case PatienSourceSource.Clinic:
                activeHealingClinicPatient.Add(newPatient);
                break;
            case PatienSourceSource.FieldHaspital:
                activeHealingHospitalPatient.Add(newPatient);
                break;
            default:
                Debug.LogWarning("Unsupported patient source. Patient not added.");
                return;
        }

        Debug.Log($"Added new patient with ID: {npcId} to {source} healing list.");
    }

    private void Start()
    {
        if (npcManager == null)
        {
            npcManager = FindObjectOfType<NpcManager>();
        }
    }

    public void UpdateJobs(List<CurePatient> jobList)
    {
        // Assume 2 HP per minute healing (implementation depends on your requirement)
        float healingPerSecond = 2f / 60f;
        
        for (int i = jobList.Count - 1; i >= 0; i--)
        {
            CurePatient job = jobList[i];
            if (!job.isfullyhealed)
            {
                job.Npchp += healingPerSecond * Time.deltaTime;

                if (job.Npchp >= 100)
                {
                    job.isfullyhealed = true;
                    CompleteHealingPatient(job);
                    jobList.RemoveAt(i);
                }
            }
        }
    }

    private void CompleteHealingPatient(CurePatient job)
    {
        // Once the patient is fully healed, they should be moved back to the normal NPC list
        // and set active again.
        if (npcManager != null)
        {
            npcManager.MoveNpcBackToNormalList(job.NpcID);
            Debug.Log("NPC " + job.NpcID + " has been fully healed and returned to normal list.");
        }
        else
        {
            Debug.LogWarning("NpcManager is not assigned. Cannot move NPC back to normal list.");
        }
    }
}

[System.Serializable]
public class CurePatient
{
    public int NpcID;
    public float Npchp;
    public float Healingrate;
    public bool isfullyhealed;
    public PatienSourceSource source;

    public CurePatient(int ID, float PaitenNpchp, float HealinfSpeed, PatienSourceSource patienSourceSource)
    {
        NpcID = ID;
        Npchp = PaitenNpchp;
        Healingrate = HealinfSpeed;
        isfullyhealed = false;
        source = patienSourceSource;
    }
}

public enum PatienSourceSource
{
    Clinic,
    FieldHaspital,
    // Add other sources as needed
}

