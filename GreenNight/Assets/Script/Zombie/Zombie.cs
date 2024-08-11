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
        FindClosestBarrier();
        currentSpeed = maxSpeed;
        currentHp = maxHp;
        rb2D = GetComponent<Rigidbody2D>();
        CheckDirectionMove();
    }

    public void ZombieAttack()
    {
        if (barrier != null)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                barrier.BarrierTakeDamage(attackDamage);
                countTImer = attackTimer;
            }
        }
    }

    public void CheckDirectionMove()
    {
        Vector2 toBarrier = transformbarrier.position - transform.position;

        if (Mathf.Abs(toBarrier.x) > Mathf.Abs(toBarrier.y))
        {
            // Move horizontally
            if (toBarrier.x > 0)
            {
                direction = Vector2.right;  // Move right
            }
            else
            {
                direction = Vector2.left;   // Move left
            }
        }
        else
        {
            // Move vertically
            if (toBarrier.y > 0)
            {
                direction = Vector2.up;     // Move up
            }
            else
            {
                direction = Vector2.down;   // Move down
            }
        }
    }

    public void FindClosestBarrier()
    {
        Barrier[] barriers = FindObjectsOfType<Barrier>();
        float closestDistance = Mathf.Infinity;
        Transform closestBarrierTransform = null;

        foreach (Barrier b in barriers)
        {
            float distanceToBarrier = Vector2.Distance(transform.position, b.transform.position);
            if (distanceToBarrier < closestDistance)
            {
                closestDistance = distanceToBarrier;
                closestBarrierTransform = b.transform;
            }
        }

        if (closestBarrierTransform != null)
        {
            transformbarrier = closestBarrierTransform;
            barrier = transformbarrier.GetComponent<Barrier>();
        }
    }

    public void ZombieMoveFindBarrier()
    {
        rb2D.velocity = direction * currentSpeed;
    }

    public void ZombieTakeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Barrier triggerbarrier = other.GetComponent<Barrier>();
        if (triggerbarrier != null)
        {
            barrier = triggerbarrier;
        }
    }
}
