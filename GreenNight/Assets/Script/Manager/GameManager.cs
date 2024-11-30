using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NpcManager npcManager;
    private void Awake() {
        npcManager = GetComponent<NpcManager>();
    }
    public void NewGame()
    {
        npcManager.StartGameCreateGropNpx();
    }
    public void LoadGane()
    {
        
    }
}
