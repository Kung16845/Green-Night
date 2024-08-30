using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : ItemClass
{   
    [Header("Stat Weapon")]
    public int rateOfFire;
    public int handling; 
    public float accuracy; 
    public int capacity; 
    public float stability; 
    public float damage; 
    public bool fullAuto; 
    public bool isShotgun;
    public float damageDropOff;
    public string ammoType;
    
}