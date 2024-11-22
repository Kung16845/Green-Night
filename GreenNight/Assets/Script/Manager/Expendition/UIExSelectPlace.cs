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
    public float riskValue;
    public int indexSceneExpendition;
    public Button buttonWalk;
    public Button buttonCar;

    public void SetInfoPlaceSelect(DataExpendition dataExpendition)
    {
        textNamePlace.text = dataExpendition.namePlace;
        textDescriptPlace.text = dataExpendition.infoDescriptPlace;
        textETA.text = dataExpendition.infoETA;
        imagePlace.sprite = dataExpendition.spriteImagePlace;
        riskValue = dataExpendition.riskEvent;
        indexSceneExpendition = dataExpendition.indexSceneExpendition;
        SetButton(buttonCar,dataExpendition.timescaleCar);
        SetButton(buttonWalk,dataExpendition.timescaleWalk);

    }
    public void SetButton(Button button,float timescale)
    {   
      
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ExpenditionManager.Instance.CreateInventorySetExpendition(timescale, riskValue,indexSceneExpendition));
    }
}
