using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUsingDDA : MonoBehaviour
{
    public bool isUsingDDA; 
    public MainSpawner mainSpawner;
    public void USEDDA()
    {
        isUsingDDA = true;
    }
    public void NOTUSEDDA()
    {
        isUsingDDA = false;
    }
}
