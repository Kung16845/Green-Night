using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public float Actionspeed;
    public bool canwalk;
    public bool canuseweapond;
    public bool candoaction;

    void Update()
    {
        if(!canuseweapond)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                canuseweapond = true;
                canwalk = true;
            }
        }
    }
}
