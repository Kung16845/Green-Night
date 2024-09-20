using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : ItemClass
{   
    [Header("Quality")]
    public int Repairmaterial;
    public float Quality;  
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
    public int Pellets;
    public float Spreadangle;
    public Ammotype ammoType; 
}

public enum Ammotype
{
    HighCaliber, 
    MediumCaliber,
    LowCaliber,
    Shotgun
}
