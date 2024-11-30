using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ItemStatDisplay : MonoBehaviour
{
    [Header("Item Reference")]
    public ItemClass item;

    [Header("UI Prefabs")]
    public GameObject statPanelPrefab; // The panel prefab to display stats
    public GameObject statElementPrefab; // The prefab for individual stat elements
    public Transform statPanelParent;
    private GameObject statPanelInstance;
    private StatPanelUI statPanelUI;

    private void Start()
    {
        // If the item reference is not set, try to get it from the GameObject
        if (item == null)
        {
            item = GetComponent<ItemClass>();
            if (item == null)
            {
                Debug.LogWarning("ItemClass component not found on the GameObject.");
            }
        }

        // Optionally, you can add a listener to the Button component here
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogWarning("Button component not found on the GameObject.");
        }
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        if (statPanelInstance == null)
        {
            ShowStatPanel();
        }
        else
        {
            CloseStatPanel();
        }
    }

    private void ShowStatPanel()
{
    // Check if the statPanelParent is set, default to the root of the canvas
    if (statPanelParent == null)
    {
        statPanelParent = FindObjectOfType<Canvas>().transform;
        if (statPanelParent == null)
        {
            Debug.LogError("No Canvas found in the scene. Please assign a parent for the stat panel.");
            return;
        }
    }

    // Instantiate the stat panel as a child of the specified parent
    statPanelInstance = Instantiate(statPanelPrefab, statPanelParent);

    // The rest of your ShowStatPanel code remains unchanged
    statPanelUI = statPanelInstance.GetComponent<StatPanelUI>();
    if (statPanelUI == null)
    {
        Debug.LogError("StatPanelUI script not found on the stat panel prefab.");
        return;
    }

    statPanelUI.itemImage.sprite = item.itemIcon;  // Use item.itemIcon
    statPanelUI.itemNameText.text = item.nameItem;

    Dictionary<string, float> itemStats = item.GetStats();
    Dictionary<string, float> maxStatValues = item.GetMaxStatValues();

    foreach (var stat in itemStats)
    {
        string statName = stat.Key;
        float statValueFloat = stat.Value;
        float maxStatValue = maxStatValues.ContainsKey(statName) ? maxStatValues[statName] : 100f;

        GameObject statElementInstance = Instantiate(statElementPrefab, statPanelUI.statContainer);

        StatElementUI statElementUI = statElementInstance.GetComponent<StatElementUI>();
        if (statElementUI == null)
        {
            Debug.LogError("StatElementUI script not found on the stat element prefab.");
            continue;
        }

        statElementUI.statText.text = $"{statName}: {statValueFloat}";
        statElementUI.statSlider.maxValue = maxStatValue;
        statElementUI.statSlider.value = Mathf.Clamp(statValueFloat, 0, maxStatValue);
    }

    StartCoroutine(DetectOutsideClick());
}


    private void CloseStatPanel()
    {
        if (statPanelInstance != null)
        {
            Destroy(statPanelInstance);
            statPanelInstance = null;
            statPanelUI = null;
        }
    }

    private IEnumerator DetectOutsideClick()
    {
        // Wait until next frame to avoid immediate closing
        yield return null;

        bool clickedOutside = false;
        while (!clickedOutside)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (statPanelInstance == null || !RectTransformUtility.RectangleContainsScreenPoint(
                    statPanelInstance.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    Camera.main))
                {
                    clickedOutside = true;
                    CloseStatPanel();
                }
            }
            yield return null;
        }
    }
}
