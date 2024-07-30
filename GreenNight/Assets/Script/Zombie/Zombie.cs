using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform transformbarrier;
    public float currentHp;
    public float maxHp;
    public float currentSpeed;
    public float maxSpeed;
    public float attackDamage;
    public float attackTimer;
    public float countTImer;
    public Vector2 direction;
    public Rigidbody2D rb2D;
    public Barrier barrier;
    private void Awake()
    {
        transformbarrier = FindObjectOfType<Barrier>().gameObject.transform;
        currentSpeed = maxSpeed;
        currentHp = maxHp;
        rb2D = GetComponent<Rigidbody2D>();
        CheckDirectionMovw();
    }
    public void ZombieAttack()
    {
        if(barrier != null)
        {
            if(countTImer > 0)
                countTImer -= Time.deltaTime;
            else 
            {
                barrier.BarrierTakeDamage(attackDamage);
                countTImer = attackTimer;
            }
        }
    }
    public void CheckDirectionMovw()
    {
        Vector2 toBarrier = transformbarrier.position - transform.position;

        // Determine the primary direction to move in
        if (Mathf.Abs(toBarrier.x) > Mathf.Abs(toBarrier.y))
        {
            // Move horizontally
            if (toBarrier.x > 0)
            {
                direction = Vector2.right;
            }
            else
            {
                direction = Vector2.left;
            }
        }
        else
        {
            // Move vertically
            if (toBarrier.y > 0)
            {
                direction = Vector2.up;
            }
            else
            {
                direction = Vector2.down;
            }
        }
    }
    public void ZombieMoveFindBarrier()
    {
        rb2D.velocity = direction * currentSpeed;
    }
    public void ZombieTakeDamage(float damage)
    {
        currentHp -= damage;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Barrier triggerbarrier = other.GetComponent<Barrier>();
        if(triggerbarrier != null)
        {
            barrier = triggerbarrier;
        }
    }
    
}
