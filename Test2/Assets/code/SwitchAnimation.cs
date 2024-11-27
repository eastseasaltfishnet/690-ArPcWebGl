// Filename: SwitchAnimation.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchAnimation : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    private GameObject currentPrefab;
    public Transform trackingTarget;
    public Button placeButton;
    public TMP_Dropdown animationDropdown;

    void Start()
    {
        // Start with no instantiated prefab
        currentPrefab = null;

        // Add listener to placeButton
        if (placeButton != null)
        {
            placeButton.onClick.AddListener(SwitchPrefab);
        }
        else
        {
            Debug.LogError("PlaceButton is not assigned.");
        }

        // Add listener to animationDropdown
        if (animationDropdown != null)
        {
            animationDropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });
        }
        else
        {
            Debug.LogError("AnimationDropdown is not assigned.");
        }
    }

    void Update()
    {
        // Removed space bar input check as we are now using the button
    }

    void SwitchPrefab()
    {
        // Destroy the current prefab if it exists
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }

        // Instantiate the selected prefab as a child of the tracking target
        if (animationDropdown != null)
        {
            switch (animationDropdown.value)
            {
                case 0:
                    currentPrefab = Instantiate(prefab1, trackingTarget);
                    break;
                case 1:
                    currentPrefab = Instantiate(prefab2, trackingTarget);
                    break;
                default:
                    Debug.LogError("Invalid dropdown value.");
                    break;
            }
        }
    }

    void UpdatePrefabSelection()
    {
        // Update prefab selection based on dropdown value
        Debug.Log("Prefab selection updated to: " + animationDropdown.options[animationDropdown.value].text);
    }

    void OnDropdownValueChanged()
    {
        // Destroy the current prefab when dropdown value changes
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }

        UpdatePrefabSelection();
    }
}
