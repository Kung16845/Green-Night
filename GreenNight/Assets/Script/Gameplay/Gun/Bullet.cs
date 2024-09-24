using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage ;
    private void OnTriggerEnter2D(Collider2D other) 
    {   
        Zombie zombie = other.GetComponent<Zombie>();
        if(zombie != null)
        {
            zombie.ZombieTakeDamage(damage, DamageType.Bullet);
            Destroy(this.gameObject);
        }
    }
}
