using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigguy : Enemy
{
    public Barrier barrier;
    void Update()
    {
       

        direction = (target.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed;

        // if (!this.GetComponent<Collider2D>().enabled)
        if (barrier != null)
        {

            if (countTImer > 0 && barrier != null)
                countTImer -= Time.deltaTime;
            else
            {
                // this.GetComponent<Collider2D>().enabled = true;
                barrier.TakeDamageBarrierServerRpc(damage);
                countTImer = attackTimer;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var objBarrier = other.GetComponent<Barrier>();
        // Debug.Log(objBarrier);
        if (objBarrier != null)
        {
            // objBarrier.TakeDamageBarrierServerRpc(damage);
            // this.GetComponent<Collider2D>().enabled = false;
            barrier = objBarrier;
        }

    }
}
