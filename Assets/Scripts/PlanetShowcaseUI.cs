using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetShowcaseUI : MonoBehaviour
{
    public static PlanetShowcaseUI instance;

    [SerializeField] private TMP_Text planetNameText;
    [SerializeField] private TMP_Text planetDescText;
    [SerializeField] private TMP_Text planetTiltText;
    [SerializeField] private TMP_Text planetDistText;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowCanvas()
    {
        GetComponent<Canvas>().enabled = true;
    }    
    
    public void HideCanvas()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void SetUIInformation(GameObject planet)
    {
        Planet planetInfo = planet.GetComponent<Orbit>().PlanetInformation;

        planetNameText.text = planetInfo.PlanetName;
        planetDescText.text = planetInfo.PlanetDesc;
        planetTiltText.text = planetInfo.PlanetName + " is at a " + planetInfo.PlanetTilt + " Degree Tilt";
        planetDistText.text = planetInfo.PlanetName + " is " + planetInfo.PlanetDist + " from the sun";
    }
}
