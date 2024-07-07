using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float currentHp;
    public float maxHp;
    public Vector2 direction;
    public Transform target;
    public float speed;
    public float currentspeed;
    public Rigidbody2D rb;
    public float damage;
    public float attackTimer;
    public float countTImer;
    public SpawnMonster spawnMonster;
    public float destroyDelay = 2f;
    public float value;
    void Start()
    {
        var objBarrier = FindObjectOfType<Barrier>().gameObject;
        // pointBarrier = new Vector2(objBarrier.transform.position.x, objBarrier.transform.position.y);
        target = objBarrier.transform;

        currentspeed = speed;
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        spawnMonster = FindObjectOfType<SpawnMonster>();

    }



    
    public void TakeDamageEnemy(float damage)
    {
        this.currentHp -= damage;

        if (this.currentHp <= 0)
        {
            StartCoroutine(DestroyAfterDelayCoroutine());
        }

    }
    private IEnumerator DestroyAfterDelayCoroutine()
    {
        currentspeed = 0;
        yield return new WaitForSeconds(destroyDelay);
      
        Destroy(gameObject);
    }
   
    
}
