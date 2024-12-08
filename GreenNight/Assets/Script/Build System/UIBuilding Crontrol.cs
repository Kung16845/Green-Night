using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingCrontrol : MonoBehaviour
{
    public UIBuilding  largeuIBuilding;
    void Start()
    {
       largeuIBuilding = FindObjectOfType<UIBuilding >();
    }
    void OnEnable()
    {
        largeuIBuilding.DisableColliders();
    }
    void OnDisable()
    {
        largeuIBuilding.EnableColliders();
    }
}
