using UnityEngine;

public enum ARState { Initialising, Scanning, Localised, TrackingLost }

public class ARStateManager : MonoBehaviour
{
    public ARState currentState { get; private set; } = ARState.Initialising;

    [Header("State UI Panels")]
    public GameObject initialisingUI;
    public GameObject scanningUI;
    public GameObject localisedUI;
    public GameObject trackingLostUI;

    void Start()
    {
        SetState(ARState.Initialising);
    }

    // Wire to MapLocalizationManager -> Localization Init () and Localization Requested ()
    public void OnLocalizationInit()
    {
        SetState(ARState.Scanning);
    }

    // Wire to MapLocalizationManager -> Localization Success ()
    public void OnLocalizationSuccess()
    {
        SetState(ARState.Localised);
    }

    // Wire to MapLocalizationManager -> Localization Failure ()
    public void OnLocalizationFailure()
    {
        if (currentState == ARState.Localised)
            SetState(ARState.TrackingLost);
        else
            SetState(ARState.Scanning); // still trying initial lock, keep showing Scanning
    }

    void SetState(ARState newState)
    {
        currentState = newState;
        Debug.Log("AR State changed to: " + newState);

        if (initialisingUI != null) initialisingUI.SetActive(newState == ARState.Initialising);
        if (scanningUI != null) scanningUI.SetActive(newState == ARState.Scanning);
        if (localisedUI != null) localisedUI.SetActive(newState == ARState.Localised);
        if (trackingLostUI != null) trackingLostUI.SetActive(newState == ARState.TrackingLost);
    }
}