using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCostumeSet : MonoBehaviour
{
    public Sprite spriteHead;
    public Sprite spriteBody;
    public Sprite spriteTopArm;
    public Sprite spriteBackArm;
    public Sprite spriteTopLeg;
    public Sprite spriteBackLeg;
    public Zombie zombie;
    public CoustumeZombieManager coustumeZombieManager;
    private void Awake()
    {
        coustumeZombieManager = FindObjectOfType<CoustumeZombieManager>();
        zombie  = GetComponent<Zombie>();
        coustumeZombieManager.SetCostumeZombie(zombie,this);
    }
}
