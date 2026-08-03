using UnityEngine;
using TMPro;
using MultiSet;

/**
 * Minimal navigation UI: shows remaining distance and an arrival message.
 * Works directly with NavigationController + PathEstimationUtils,
 * bypassing NavigationUIController's list/panel dependencies.
 */
public class SimpleNavUI : MonoBehaviour
{
    [Tooltip("Label to show remaining distance")]
    public TextMeshProUGUI remainingDistance;

    [Tooltip("Label to show current destination name")]
    public TextMeshProUGUI destinationName;

    [Tooltip("Optional: Stop navigation button")]
    public GameObject stopButton;

    [Tooltip("Optional: text/panel shown briefly on arrival")]
    public GameObject arrivedMessage;

    [Tooltip("How long the arrival message stays visible (seconds)")]
    public float arrivedMessageDuration = 3f;

    void Start()
    {
        // Listen for arrival, fired from NavigationController's UnityEvent
        NavigationController.instance.DestinationArrived.AddListener(OnArrived);

        SetNavigatingUI(false);
        if (arrivedMessage != null) arrivedMessage.SetActive(false);
    }

    void Update()
    {
        bool navigating = NavigationController.instance.IsCurrentlyNavigating();
        SetNavigatingUI(navigating);

        if (navigating)
        {
            if (destinationName != null)
                destinationName.text = NavigationController.instance.currentDestination.poiName;

            if (remainingDistance != null)
            {
                int distance = PathEstimationUtils.instance.getRemainingDistanceMeters();
                remainingDistance.text = distance + " m remaining";
            }
        }
        else
        {
            if (destinationName != null) destinationName.text = "";
            if (remainingDistance != null) remainingDistance.text = "";
        }
    }

    void SetNavigatingUI(bool isVisible)
    {
        if (stopButton != null) stopButton.SetActive(isVisible);
    }

    // Called on stop button's OnClick()
    public void OnStopPressed()
    {
        NavigationController.instance.StopNavigation();
    }

    void OnArrived()
    {
        if (arrivedMessage != null)
        {
            arrivedMessage.SetActive(true);
            CancelInvoke(nameof(HideArrivedMessage));
            Invoke(nameof(HideArrivedMessage), arrivedMessageDuration);
        }
    }

    void HideArrivedMessage()
    {
        arrivedMessage.SetActive(false);
    }
}