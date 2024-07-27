using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform player;
    public float currentHp;
    public float maxHp;
    public float currentSpeed;
    public float maxSpeed;
    public float attackSpeed;
    public float attackDamage;
    public Vector2 direction;
    public Rigidbody2D rb2D;
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject.transform;
        currentSpeed = maxSpeed;
        currentHp = maxHp;
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void ZombieMoveFindPlayer()
    {
        direction = (player.position - transform.position).normalized;
        rb2D.velocity = direction * currentSpeed;
    }
    public void ZombieTakeDamage(float damage)
    {
        currentHp -= damage;
    }
}
