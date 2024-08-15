using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : MonoBehaviour
{
   
    public Building building;
    public BuildManager buildManager;
    public Image image;
    public TextMeshProUGUI textNameBuild;
    public TextMeshProUGUI textDescriveBuild;

    public void SetDataBuild()
    {
        textNameBuild.text = building.nameBuild;
        textDescriveBuild.text = building.detailBuild;
        image.sprite = building.GetComponent<Sprite>();
        buildManager.building = building;
    }
}
