using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DataItems : MonoBehaviour
{
    public int iDItems;
    public string nameImage;
    public bool isResourece;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Destroy(this.gameObject);
        }
    }
}
