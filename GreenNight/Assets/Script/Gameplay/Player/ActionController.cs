using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public float actionSpeed = 1f; // Affects the speed at which the slider fills
    public bool canwalk = true;
    public bool canuseweapon = true;
    public bool canDoAction = true;

    public Slider stuckSlider; // Reference to the Slider component

    private bool isStuck = false;
    private float releasedvalue = 0f;
    private float stuckProgress = 0f; // Current progress on the slider
    public float maxStuckValue = 50f; // Value at which the player is released

    void Start()
    {
        if (stuckSlider != null)
        {
            // Activate the Slider to ensure properties are set correctly
            stuckSlider.gameObject.SetActive(true);

            // Set the max value and reset the slider
            stuckSlider.maxValue = maxStuckValue;
            stuckSlider.value = 0;

            // Deactivate the Slider until needed
            stuckSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("stuckSlider is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (isStuck)
        {
            HandleStuckState();
        }

        // Example toggle for weapon use (unrelated to slime)
        // if (!canuseweapon)
        // {
        //     if (Input.GetKeyDown(KeyCode.Q))
        //     {
        //         canuseweapon = true;
        //         canwalk = true;
        //     }
        // }
    }

    // Call this method when the player gets stuck in slime
    public void StartStuck()
    {
        isStuck = true;
        canwalk = false;
        // canuseweapon = false;
        stuckProgress = 0f;
        releasedvalue = 10f;

        // Enable the stuck UI (slider)
        if (stuckSlider != null)
        {
            stuckSlider.gameObject.SetActive(true);
            stuckSlider.value = stuckProgress;
            stuckSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void HandleStuckState()
    {
        // Check for input to increase the slider
        if (Input.GetKeyDown(KeyCode.E))
        {
            stuckProgress += releasedvalue * actionSpeed;
            Debug.Log(stuckProgress);
            if (stuckProgress > maxStuckValue)
            {
                stuckProgress = maxStuckValue;
            }

            // Update the slider UI
            if (stuckSlider != null)
            {
                stuckSlider.value = stuckProgress;
                // The OnSliderValueChanged method will be automatically called
            }
        }
    }

    // This method is called whenever the slider's value changes
    public void OnSliderValueChanged(float value)
    {
        stuckProgress = value;

        // Check if the player has filled the slider
        if (stuckProgress >= maxStuckValue)
        {
            ReleaseFromSlime();
        }
    }

    private void ReleaseFromSlime()
    {
        isStuck = false;
        canwalk = true;
        canuseweapon = true;

        // Disable the stuck UI
        if (stuckSlider != null)
        {
            stuckSlider.gameObject.SetActive(false);
            // Remove the listener if added in code
            // stuckSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        // Reset the progress
        stuckProgress = 0f;
    }
}
