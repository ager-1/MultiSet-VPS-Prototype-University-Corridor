using UnityEngine;
using UnityEngine.UI;

public class POIButtonBinder : MonoBehaviour
{
    public POI targetPOI;   // drag the matching Room POI object here

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(NavigateHere);
    }

    void NavigateHere()
    {
        NavigationController.instance.SetPOIForNavigation(targetPOI);
    }
}