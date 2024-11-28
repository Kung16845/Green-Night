using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCostumeSet : MonoBehaviour
{
    public SpriteRenderer spriteHead;
    public SpriteRenderer spriteBody;
    public SpriteRenderer spriteTopArm;
    public SpriteRenderer spriteBackArm;
    public SpriteRenderer spriteTopLeg;
    public SpriteRenderer spriteBackLeg;
    public Zombie zombie;
    public CoustumeZombieManager coustumeZombieManager;
    private void Awake()
    {
        coustumeZombieManager = FindObjectOfType<CoustumeZombieManager>();
        zombie  = GetComponent<Zombie>();
    }
    private void Start()
    {
        coustumeZombieManager.SetCostumeZombie(zombie,this);
    }
}
