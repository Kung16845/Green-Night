using System.Collections.Generic;
using UnityEngine;

public class Reeker : Zombie
{
    private enum ReekerState
    {
        Moving,
        Spitting,
        SwitchingLanes
    }

    [Header("Reeker Settings")]
    public float spittingDuration = 5f;    // Total time spent spitting acid
    public float spittingInterval = 1f;    // Time between each acid spit
    public GameObject acidPoolPrefab;      // Prefab of the AcidPool to spawn

    private ReekerState currentState = ReekerState.Moving;
    private float spittingTimer = 0f;
    private float spittingCooldown = 0f;

    // Reference to the engaging area position
    public Transform engagingPoint;

    // List of all lanes for lane switching
    public List<Lane> allLanes;

    private void Start()
    {
        // Initialize necessary components
        rb2D = GetComponent<Rigidbody2D>();
        // Initialize variables
        currentState = ReekerState.Moving;
        countTimer = attackTimer; // Initialize attack timer

        // Initialize from base class
    }

    private void Update()
    {
        switch (currentState)
        {
            case ReekerState.Moving:
                HandleMovingState();
                break;
            case ReekerState.Spitting:
                HandleSpittingState();
                break;
            case ReekerState.SwitchingLanes:
                HandleSwitchingLanesState();
                break;
            default:
                break;
        }
    }

    private void HandleMovingState()
    {
        if (HasReachedEngagingPoint())
        {
            currentState = ReekerState.Spitting;
            spittingTimer = spittingDuration;
            spittingCooldown = 0f;
            rb2D.velocity = Vector2.zero; // Stop moving while spitting
        }
        else
        {
            // Move towards the engaging point
            MoveTowards(engagingPoint.position);
        }
    }

    private void HandleSpittingState()
    {
        spittingTimer -= Time.deltaTime;
        spittingCooldown -= Time.deltaTime;

        if (spittingCooldown <= 0f)
        {
            SpitAcid();
            spittingCooldown = spittingInterval;
        }

        if (spittingTimer <= 0f)
        {
            currentState = ReekerState.SwitchingLanes;
        }
    }

    private void HandleSwitchingLanesState()
    {
        SwitchToRandomLane();
        currentState = ReekerState.Moving;
    }

    private bool HasReachedEngagingPoint()
    {
        if (engagingPoint != null)
        {
            float distanceToEngagingPoint = Vector2.Distance(transform.position, engagingPoint.position);
            float thresholdDistance = 0.1f;
            return distanceToEngagingPoint <= thresholdDistance;
        }
        return false;
    }

    private void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb2D.velocity = direction * currentSpeed;
    }
    private void SpitAcid()
    {
        if (acidPoolPrefab != null)
        {
            GameObject acidPool = Instantiate(acidPoolPrefab, transform.position, Quaternion.identity);
            AcidPool acidPoolScript = acidPool.GetComponent<AcidPool>();
            if (acidPoolScript != null)
            {
                // Configure the acid pool with correct parameter names
                acidPoolScript.Initialize(
                    barrierDamage: 5f,   // Adjust these values as needed
                    zombieDamage: 5f,
                    duration: 5f,
                    radius: 1f,
                    interval: 1f
                );
            }
        }
    }
    private void SwitchToRandomLane()
    {
        if (allLanes != null && allLanes.Count > 1)
        {
            // Exclude current lane from selection
            List<Lane> otherLanes = new List<Lane>(allLanes);
            otherLanes.Remove(currentLane);

            // Select a random lane
            int randomIndex = Random.Range(0, otherLanes.Count);
            Lane newLane = otherLanes[randomIndex];

            // Update the current lane and engaging point
            SetLane(newLane);
            engagingPoint = newLane.engagingArea;
        }
        else
        {
            // No other lanes to switch to
            currentState = ReekerState.Moving;
        }
    }

}
