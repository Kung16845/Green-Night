using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float baseSprintSpeed = 8f;

    public float baseMaxStamina = 100f;
    public float currentStamina;
    public float baseStaminaRecoverSpeed = 10f;
    public float baseStaminaConsumeSpeed = 15f;
    public float minStaminaToSprint = 30f;

    private bool isMovementStopped = false;
    private bool isSprinting = false;
    private bool canSprint = true; // Track if player can start sprinting again
    private ActionController actionController;
    private AnimationController animationController;
    private StatAmplifier statAmplifier;

    void Start()
    {
        statAmplifier = GetComponent<StatAmplifier>();
        actionController = GetComponent<ActionController>();
        animationController = GetComponent<AnimationController>();
        if (statAmplifier == null)
        {
            Debug.LogError("StatAmplifier component not found on the player.");
        }

        // Initialize current stamina to max stamina at the start
        currentStamina = GetMaxStamina();
    }

    void Update()
    {
        if (actionController != null && actionController.canwalk)
        {
            HandleMovement();
            HandleStamina();
        }
    }

    private void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        float speedMultiplier = statAmplifier.GetSpeedMultiplier();
        float movementSpeed = baseSpeed * speedMultiplier;
        float sprintMovementSpeed = baseSprintSpeed * speedMultiplier;
        
        if (direction.magnitude > 0) // Check if player is moving
        {
            // Flip character's direction based on horizontal input
            if (horizontal != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = horizontal > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

            // Check if the player is sprinting
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && canSprint)
            {
                animationController.isrun = true;
                animationController.iswalk = false;
                isSprinting = true;
                transform.Translate(direction * sprintMovementSpeed * Time.deltaTime);
            }
            else
            {
                animationController.iswalk = true;
                animationController.isrun = false;
                isSprinting = false;
                transform.Translate(direction * movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            animationController.iswalk = false;
            animationController.isrun = false;
        }
    }


    private void HandleStamina()
    {
        float staminaConsumeMultiplier = statAmplifier.GetStaminaConsumeMultiplier();
        float staminaRecoverMultiplier = statAmplifier.GetStaminaRecoverMultiplier();

        if (isSprinting)
        {
            // Consume stamina while sprinting
            currentStamina -= baseStaminaConsumeSpeed * staminaConsumeMultiplier * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false; // Disable sprinting until stamina recovers
                isSprinting = false; // Stop sprinting when stamina is depleted
            }
        }
        else
        {
            // Recover stamina when not sprinting
            currentStamina += baseStaminaRecoverSpeed * staminaRecoverMultiplier * Time.deltaTime;
            if (currentStamina > GetMaxStamina())
            {
                currentStamina = GetMaxStamina();
            }

            // Allow sprinting again if stamina reaches the minimum required level
            if (currentStamina >= minStaminaToSprint)
            {
                canSprint = true;
            }
        }
    }

    public float GetMaxStamina()
    {
        return baseMaxStamina * statAmplifier.GetMaxStaminaMultiplier();
    }

    public void StopMovementForDuration(float duration)
    {
        StartCoroutine(StopMovementCoroutine(duration));
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        isMovementStopped = true;
        yield return new WaitForSeconds(duration);
        isMovementStopped = false;
    }
}
