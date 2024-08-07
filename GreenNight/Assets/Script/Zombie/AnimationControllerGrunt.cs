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
    public ZombieGrunt zombieGrunt;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsAttack = false;
        IsDead = false;
        IsWalk = true;
        zombieGrunt = GetComponent<ZombieGrunt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zombieGrunt == null)
        {
            Debug.LogWarning("zombieGrunt component not found!");
            return;
        }

        if (zombieGrunt.countTImer == 0)
        {
            IsWalk = true;
            animator.SetBool("Isreach", false);
        }
        else
        {
            animator.SetBool("Isreach", true);
        }

        if (zombieGrunt.currentHp <= 0 && !IsDead)
        {
            IsDead = true;
            animator.SetBool("Isdead", IsDead);
        }
    }
}
