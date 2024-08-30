using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ItemData 
{   
    public string nameItem;
    public int idItem;
    public Itemtype itemtype;
}
public enum Itemtype
{   
    Backpack,
    Grenade,
    Tool,
    Vest,
    Weapon
}