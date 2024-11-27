// Filename: SwitchAnimation.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchAnimation : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    private GameObject currentPrefab;
    public Transform trackingTarget;
    public Button placeButton;

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

        // Instantiate the appropriate prefab as a child of the tracking target
        if (currentPrefab == prefab1 || currentPrefab == null)
        {
            currentPrefab = Instantiate(prefab2, trackingTarget);
        }
        else
        {
            currentPrefab = Instantiate(prefab1, trackingTarget);
        }
    }
}
