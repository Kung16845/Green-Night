using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterpump : MonoBehaviour
{
    public BuildManager buildManager;
    public Building building;
    void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        building = GetComponent<Building>();
    }
    void Update()
    {
        Activewater();
    }
    void Activewater()
    {
        if(building.isfinsih)
        {
            buildManager.iswateractive = true;
        }
    }
}
