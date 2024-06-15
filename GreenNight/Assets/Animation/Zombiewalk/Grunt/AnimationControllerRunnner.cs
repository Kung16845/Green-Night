using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerRunner : MonoBehaviour
{   
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool IsAttack;
    public bool IsDead;
    public bool IsWalk;
    public EnemySpeedter enemySpeedter;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsAttack = false;
        IsDead = false;
        IsWalk = true;
        enemySpeedter = GetComponent<EnemySpeedter>();
    }

    void Update()
    {
      

        if (enemySpeedter == null)
        {
            Debug.LogWarning("EnemyGrunt component not found!");
            return;
        }

        if (enemySpeedter.countTImer == 0)
        {
            IsWalk = true;
            animator.SetBool("Isreach", false);
        }
        else
        {
            animator.SetBool("Isreach", true);
        }

        if (enemySpeedter.currentHp <= 0 && !IsDead)
        {
            IsDead = true;
            animator.SetBool("Isdead", IsDead);
        }
    }
}
