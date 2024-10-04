using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float currentSpeed = 5f;
    public float sprintSpeed = 8f;
    
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRecoverSpeed = 10f;
    public float staminaConsumeSpeed = 15f;
    public float minStaminaToSprint = 10f;
    
    private bool isMovementStopped = false;
    private bool isSprinting = false;
    
    private StatAmplifier statAmplifier;

    void Start()
    {
        // Initialize current stamina to max stamina at the start
        statAmplifier = GetComponent<StatAmplifier>();
        ApplyStatAmplifier();
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (!isMovementStopped)
        {
            HandleMovement();
            HandleStamina();
        }
    }

    private void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(horizontal, vertical);
        
        // Check if the player is pressing the sprint key (Shift) and has enough stamina
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > minStaminaToSprint)
        {
            isSprinting = true;
            transform.Translate(direction * sprintSpeed * statAmplifier.GetSpeedMultiplier() * Time.deltaTime);
        }
        else
        {
            isSprinting = false;
            transform.Translate(direction * currentSpeed * statAmplifier.GetSpeedMultiplier() * Time.deltaTime);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaConsumeSpeed * Time.deltaTime;
            if (currentStamina < 0f)
            {
                currentStamina = 0f;
            }
        }
        else
        {
            currentStamina += staminaRecoverSpeed * statAmplifier.GetSpeedMultiplier() * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
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
    private void ApplyStatAmplifier()
    {
        maxStamina *= statAmplifier.GetEnduranceMultiplier();
    }
}
