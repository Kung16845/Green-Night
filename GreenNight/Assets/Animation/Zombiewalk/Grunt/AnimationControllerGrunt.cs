using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerGrunt : MonoBehaviour
{   
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool IsAttack;
    public bool IsDead;
    public bool IsWalk;
    public EnemyGrunt enemyGrunt;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsAttack = false;
        IsDead = false;
        IsWalk = true;
        enemyGrunt = GetComponent<EnemyGrunt>();
    }

    void Update()
    {
        

        if (enemyGrunt == null)
        {
            Debug.LogWarning("EnemyGrunt component not found!");
            return;
        }

        if (enemyGrunt.countTImer == 0)
        {
            IsWalk = true;
            animator.SetBool("Isreach", false);
        }
        else
        {
            animator.SetBool("Isreach", true);
        }

        if (enemyGrunt.currentHp <= 0 && !IsDead)
        {
            IsDead = true;
            animator.SetBool("Isdead", IsDead);
        }
    }
}
