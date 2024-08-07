using System;
using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.5f; // Speed multiplier when sprinting
    public float staminaMax = 100f; // Maximum stamina
    public float staminaDepletionRate = 25f; // Stamina points per second when sprinting
    public float staminaRegenRate = 5f; // Stamina points regained per second when not sprinting

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool isWalking;
    public float currentStamina;
    public float currentSpeed; // Track current speed

    private void Start()
    {
        currentStamina = staminaMax;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isWalking = false;
    }

    private void FixedUpdate()
    {
        
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(vertical) > 0.1f || Mathf.Abs(horizontal) > 0.1f)
            {
                isWalking = true;
                animator.SetBool("iswalk", true);
                // Flip the sprite if moving left
                if (horizontal < 0)
                {
                    spriteRenderer.flipX = true;
                    FlipSprite(true);
                }
                else if (horizontal > 0)
                {
                    spriteRenderer.flipX = false;
                    FlipSprite(false);
                }
            }
            else
            {
                isWalking = false;
                animator.SetBool("iswalk", false);
            }
            Vector2 direction = new Vector2(horizontal, vertical);

            if (Input.GetKey(KeyCode.LeftShift) && currentStamina >= 1f)
            {
                // Sprinting
                currentSpeed = speed * sprintMultiplier;
                currentStamina -= staminaDepletionRate * Time.deltaTime;
            }
            else
            {
                // Not sprinting - normal speed
                currentSpeed = speed;
                if (!Input.GetKey(KeyCode.LeftShift))
                    currentStamina += staminaRegenRate * Time.deltaTime;
            }

            // Apply movement
            transform.Translate(direction * currentSpeed * Time.deltaTime);

            // Clamp stamina within [0, staminaMax]
            currentStamina = Mathf.Clamp(currentStamina, 0f, staminaMax);
        
    }

    public void StopMovemen()
    {
        speed = 0f;
    }

    private void FlipSprite(bool flipX)
    {
        spriteRenderer.flipX = flipX;
        
    }


   
}
