using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEX : MonoBehaviour
{
    public TextMeshProUGUI textDayHourFinish;
    public Image imageHead;
    public int indexEXUI;
    public ExpenditionManager expenditionManager;
    private void Start()
    {   
        expenditionManager = FindObjectOfType<ExpenditionManager>();
        if(indexEXUI == 1)
        {
            expenditionManager.uIButtonEXOne = this;
        }
        else 
        {
            expenditionManager.uIButtonEXTwo = this;
        }
    }

    public void SetUIButtonEX(Sprite spriteHead, string finishDayHour)
    {
        textDayHourFinish.text = finishDayHour;
        imageHead.sprite = spriteHead;
    }
}
