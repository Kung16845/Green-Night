using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public float currentHp;
    public float maxHp;
    private void Start() {
        currentHp = maxHp;
    }
}
