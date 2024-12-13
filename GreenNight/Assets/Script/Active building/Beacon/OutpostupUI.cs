using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostupUI : MonoBehaviour
{
    public Outpostup outpostup;
    public Globalstat globalstat;
    public OutpostSystem outpostSystem;
    public GameObject SetupoutpostButton;
    public GameObject DisableButton;
    void Start()
    {
        outpostSystem = FindObjectOfType<OutpostSystem>();
        globalstat = FindObjectOfType<Globalstat>();
    }
    void Update()
    {
        if(globalstat.OutpostLimit > outpostSystem.outpostRewards.Count && !outpostup.issetUpOutpost)
        {
            SetupoutpostButton.SetActive(true);
            DisableButton.SetActive(false);
        }
        else if(outpostup.issetUpOutpost)
        {
            SetupoutpostButton.SetActive(false);
            DisableButton.SetActive(true);
        }

    }
}
