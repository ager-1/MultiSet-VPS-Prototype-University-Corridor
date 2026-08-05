using UnityEngine;
using UnityEngine.UI;

public class POIButtonBinder : MonoBehaviour
{
    public POI targetPOI;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(NavigateHere);
    }

    void NavigateHere()
    {
        NavigationController.instance.SetPOIForNavigation(targetPOI);
    }
}