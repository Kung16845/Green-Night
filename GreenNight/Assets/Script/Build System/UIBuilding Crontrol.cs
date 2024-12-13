using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingCrontrol : MonoBehaviour
{
    public BuildManager  buildManager;
    void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
    }
    void OnEnable()
    {
        buildManager.DisableColliders();
    }
    void OnDisable()
    {
        buildManager.EnableColliders();
    }
}
