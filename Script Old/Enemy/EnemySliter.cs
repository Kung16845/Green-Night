using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySliter : Enemy
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        direction = (target.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed;

        if (!this.GetComponent<Collider2D>().enabled)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                this.GetComponent<Collider2D>().enabled = true;

                countTImer = attackTimer;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Movement>();
        // Debug.Log(objBarrier);
        if (player != null)
        {
           
            this.GetComponent<Collider2D>().enabled = false;
        }

    }
}
