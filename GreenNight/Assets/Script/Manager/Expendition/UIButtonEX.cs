using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEX : MonoBehaviour
{
    public TextMeshProUGUI textDayHourFinish;
    public Image imageHead;
    public void SetUIButtonEX(Sprite spriteHead,string finishDayHour)
    {
        textDayHourFinish.text = finishDayHour;
        imageHead.sprite = spriteHead;
    }
}
