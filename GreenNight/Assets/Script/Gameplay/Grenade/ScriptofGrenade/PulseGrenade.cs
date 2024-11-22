using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseGrenade : MonoBehaviour
{
    public float damage;
    public float Delay;
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(Delay));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>(); 
        if (zombie != null)
        {
            zombie.ZombieTakeDamage(damage, DamageType.Pulse);
        }
    }
    private IEnumerator DestroyAfterDelay(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Destroy(this.gameObject);
    }
}
