using System;
using UnityEngine;



public class Animationcontroller : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public int Whichgun;
    public bool Isaim;
    public bool isWalking;
    public float currentStamina;
    public bool isRightMousePressed;
    public bool Isshoot;
    public float currentSpeed; // Track current speed

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isWalking = false;
        Isshoot = false;
        Isaim = false;
        Whichgun = 3;
    }

    private void Update()
    {
        if(IsOwner)
        {
            if(Input.GetKey(KeyCode.Alpha3))
            {
                Whichgun = 3;
                animator.SetInteger("Whichgun", 3);
            }
            if(Input.GetKey(KeyCode.Alpha2))
            {
                Whichgun = 2;
                animator.SetInteger("Whichgun", 2);
            }
            if(Input.GetKey(KeyCode.Alpha1))
            {
                Whichgun = 1;
                animator.SetInteger("Whichgun", 1);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Isaim = true;
                animator.SetBool("Isaim", Isaim);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Isaim = false;
                animator.SetBool("Isaim", Isaim);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Isshoot = true;
                animator.SetBool("Isshoot", Isshoot);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Isshoot = false;
                animator.SetBool("Isshoot", Isshoot);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                animator.SetBool("Isthrown", true);
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                animator.SetBool("Isthrown", false);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetBool("Isreload", true);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                animator.SetBool("Isreload", false);
            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                animator.SetBool("Isrepair", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                animator.SetBool("Isrepair", false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("Isunpack", true);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                animator.SetBool("Isunpack", false);
            }
        }
    }



    private void FlipSprite(bool flipX)
    {
        spriteRenderer.flipX = flipX;
        
    }

    
   
}
