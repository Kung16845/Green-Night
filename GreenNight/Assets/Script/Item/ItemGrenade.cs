using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrenade : ItemClass
{   
    [Header("Stat Grenade")]
    public float damage;
    public bool isAOE;
    public float AoeRange;
    public float armTime;
    public float MinimumRange;
    public float Maxrange;
    public string effect;
    public string typeGrende;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
