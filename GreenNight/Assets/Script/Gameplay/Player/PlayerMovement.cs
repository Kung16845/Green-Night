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
    public float minStaminaToSprint = 10f;

    private bool isMovementStopped = false;
    private bool isSprinting = false;
    private ActionController actionController;
    private StatAmplifier statAmplifier;

    void Start()
    {
        statAmplifier = GetComponent<StatAmplifier>();
        actionController = GetComponent<ActionController>();

        if (statAmplifier == null)
        {
            Debug.LogError("StatAmplifier component not found on the player.");
        }

        // Initialize current stamina to max stamina at the start
        currentStamina = GetMaxStamina();
    }

    void Update()
    {
        if (actionController.canwalk)
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

        // Check if the player is pressing the sprint key (Shift) and has enough stamina
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > minStaminaToSprint)
        {
            isSprinting = true;
            transform.Translate(direction * sprintMovementSpeed * Time.deltaTime);
        }
        else
        {
            isSprinting = false;
            transform.Translate(direction * movementSpeed * Time.deltaTime);
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
            if (currentStamina < 0f)
            {
                currentStamina = 0f;
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
