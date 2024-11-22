using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGrenade : MonoBehaviour
{
    public float Delay;
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(Delay));
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>(); 
        if (zombie != null)
        {
            zombie.currentSpeed = 0.25f;
        }
    }
    private IEnumerator DestroyAfterDelay(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        Destroy(this.gameObject);
    }
}
