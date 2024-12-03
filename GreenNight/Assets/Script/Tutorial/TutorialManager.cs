using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(15, 20)]
        public string description; // Text to display for this tutorial step
        public GameObject highlightObject; // Object to highlight (optional)
        public Transform panelTextPosition; // Transform to set the position of the paneltext (optional)
    }

    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    public TextMeshProUGUI tutorialText; // UI Text to show tutorial descriptions
    public GameObject paneltext;
    public GameObject overlayPanel; // Optional: Panel to dim the background

    private int currentStepIndex = 0;

    void Start()
    {
        if (tutorialSteps.Count > 0)
        {
            Time.timeScale = 0f; // Pause the game
            ShowTutorialStep();
        }
        else
        {
            Debug.LogWarning("No tutorial steps defined!");
            EndTutorial();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            NextTutorialStep();
        }
    }

    private void ShowTutorialStep()
    {
        if (currentStepIndex < tutorialSteps.Count)
        {
            TutorialStep step = tutorialSteps[currentStepIndex];

            if (tutorialText != null)
            {
                paneltext.gameObject.SetActive(true);
                tutorialText.text = step.description;

                // Adjust the panel position if specified
                if (step.panelTextPosition != null)
                {
                    paneltext.transform.position = step.panelTextPosition.position;
                }
            }

            if (overlayPanel != null)
                overlayPanel.SetActive(true);

            // Highlight object if specified
            if (step.highlightObject != null)
                step.highlightObject.SetActive(true);
        }
    }

    private void NextTutorialStep()
    {
        // Deactivate current highlight object if any
        if (currentStepIndex < tutorialSteps.Count && tutorialSteps[currentStepIndex].highlightObject != null)
        {
            tutorialSteps[currentStepIndex].highlightObject.SetActive(false);
        }

        currentStepIndex++;

        if (currentStepIndex >= tutorialSteps.Count)
        {
            EndTutorial();
        }
        else
        {
            ShowTutorialStep();
        }
    }

    private void EndTutorial()
    {
        Time.timeScale = 1f; // Resume the game

        if (tutorialText != null)
        {
            paneltext.gameObject.SetActive(false);
            tutorialText.text = ""; // Clear tutorial text
        }
        if (overlayPanel != null)
            overlayPanel.SetActive(false);

        Debug.Log("Tutorial finished.");
    }
}
