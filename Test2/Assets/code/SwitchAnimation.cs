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
    public TextMeshProUGUI contentBoxText;

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
                    UpdateContentBoxText("Insert the GPU onto the first slot (with red grid) on the mother board");
                    break;
                case 1:
                    currentPrefab = Instantiate(prefab2, trackingTarget);
                    UpdateContentBoxText("press the clip to unclock the insert the RAM onto the first and third slot! After you hear the 'click!' sound from the clip you are good to go");
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

    void UpdateContentBoxText(string newText)
    {
        if (contentBoxText != null)
        {
            contentBoxText.text = newText;
        }
        else
        {
            Debug.LogError("ContentBoxText is not assigned.");
        }
    }
}
