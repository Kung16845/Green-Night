using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExSelectPlace : MonoBehaviour
{   
    public TextMeshProUGUI textNamePlace;
    public TextMeshProUGUI textDescriptPlace;
    public TextMeshProUGUI textETA;
    public Image imagePlace;
    public void SetInfoPlaceSelect(DataExpendition dataExpendition)
    {
        textNamePlace.text = dataExpendition.namePlace;
        textDescriptPlace.text = dataExpendition.infoDescriptPlace;
        textETA.text = dataExpendition.infoETA;
        imagePlace.sprite = dataExpendition.spriteImagePlace;
    }    
}
