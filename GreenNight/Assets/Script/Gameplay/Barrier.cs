using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float currentHp;
    public float maxHp;
    private void Start() 
    {
        currentHp = maxHp;
    }
    public void BarrierTakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
            Time.timeScale = 0;
    }
    public void BarrierHealDamage(float heal)
    {
        currentHp += heal;
        if (currentHp > maxHp)
            currentHp = maxHp;
    }

}
