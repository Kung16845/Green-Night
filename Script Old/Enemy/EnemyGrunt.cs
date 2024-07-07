using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGrunt : Enemy
{   
    public Barrier barrier; 
    void Update()
    {
    

        direction = (target.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed;

        
        if(barrier !=null)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                // this.GetComponent<Collider2D>().enabled = true;
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
