using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isgunequip;
    public bool iswalk;
    public bool isrun;
    void Start()
    {
        animator = GetComponent<Animator>();
        isgunequip = false;
        iswalk = false;
        isrun = false;
    }
    void Update()
    {
        if (isgunequip)
        {
            animator.SetBool("Gunequip", true);
        }
        else if(!isgunequip)
        {
            animator.SetBool("Gunequip", false);
        }
    }
}
