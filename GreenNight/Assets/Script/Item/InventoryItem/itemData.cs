using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemData 
{   
    public string nameItem;
    public int idItem;
    public int count;
    public int maxCount;
    public Itemtype itemtype;
}
public enum Itemtype
{   
    Backpack,
    Grenade,
    Tool,
    Vest,
    Weapon,
    Ammo,
    Pill,
    Gadget
}