using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpeedter : Enemy
{

    public bool isFirstAttack;
    public Barrier barrier;
    void Update()
    {
       

        direction = (target.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed;

        // if (!this.GetComponent<Collider2D>().enabled)
        if(barrier !=null)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                if (isFirstAttack)
                {
                    damage = damage * 2;
                    barrier.TakeDamageBarrier(damage);
                    isFirstAttack = false;
                    damage = damage / 2;
                }


                this.GetComponent<Collider2D>().enabled = true;
                barrier.TakeDamageBarrier(damage);
                countTImer = attackTimer;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var objBarrier = other.GetComponent<Barrier>();
        
        if (objBarrier != null)
        {
            
            barrier = objBarrier;
        }

    }
}
