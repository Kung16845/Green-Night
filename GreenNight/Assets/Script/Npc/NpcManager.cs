using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [Header("List Coutume")]
    public List<HeadCoutume> listHeadCoutume = new List<HeadCoutume>();
    public List<BodyCoutume> listBodyCoutume = new List<BodyCoutume>();
    public List<FeedCoutume> listFeedCoutume = new List<FeedCoutume>();
    [Header("Npc")]
    public List<NpcClass> listNpc = new List<NpcClass>();
    public List<NpcClass> listNpcWorking = new List<NpcClass>();

    public void CreateNpc(NpcClass npcCreate)
    {
        
    }
    public void Unlock(SpecialistNpc specialistNpc)
    {
        
    }
}
