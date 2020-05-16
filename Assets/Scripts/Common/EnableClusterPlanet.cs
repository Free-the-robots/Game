using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClusterPlanet : MonoBehaviour
{
    public FillUIParent line;
    public FillUIParent percentage;
    public List<PlanetEnabler> planets = new List<PlanetEnabler>();
    public List<GameObject> planet3D = new List<GameObject>();
    // Start is called before the first frame update
    public void enableAll()
    {
        line.enabled = true;
        percentage.enabled = true;
        GetComponent<CanvasGroupFade>().enabled = true;
        gameObject.SetActive(true);
    }

    public void ResetFills()
    {
        line.ResetFills();
        percentage.ResetFills();
        foreach (PlanetEnabler planete in planets)
            planete.ResetFills();
        foreach (GameObject gb in planet3D)
            gb.SetActive(false);
    }
}
