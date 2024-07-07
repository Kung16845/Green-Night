using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimationControllerBigguy : MonoBehaviour
{
     private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool IsAttack;
    public bool IsDead;
    public bool IsWalk;
    public EnemyBigguy enemyBigguy;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsAttack = false;
        IsDead = false;
        IsWalk = true;
        enemyBigguy = GetComponent<EnemyBigguy>();
    }

    void Update()
    {
      

        if (enemyBigguy == null)
        {
            Debug.LogWarning("EnemyGrunt component not found!");
            return;
        }

        if (enemyBigguy.countTImer == 0)
        {
            IsWalk = true;
            animator.SetBool("Isreach", false);
        }
        else
        {
            animator.SetBool("Isreach", true);
        }

        if (enemyBigguy.currentHp <= 0 && !IsDead)
        {
            IsDead = true;
            animator.SetBool("Isdead", IsDead);
        }
    }
}
