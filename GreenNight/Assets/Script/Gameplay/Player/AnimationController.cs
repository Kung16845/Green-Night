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
    public bool isfire;
    public bool isreload;
    public int Guntype;


    void Start()
    {
        animator = GetComponent<Animator>();
        isgunequip = false;
        iswalk = false;
        isrun = false;
        isfire = false;
        isreload = false;
        Guntype = 0;
    }

    void Update()
    {

        // Update gun equip animation state
        animator.SetBool("Isfire", isfire);
        animator.SetBool("Gunequip", isgunequip);
        animator.SetBool("Isreload", isreload);
        animator.SetInteger("GunType",Guntype);
        // Update movement animation states
        animator.SetBool("Iswalk", iswalk);
        animator.SetBool("Isrun", isrun);
    }
}
