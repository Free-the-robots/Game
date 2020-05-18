using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClusterPlanet : MonoBehaviour
{
    public FillUIParent line;
    public FillUIParent percentage;
    public CanvasGroupFade planetInfo;
    public List<PlanetEnabler> planets = new List<PlanetEnabler>();
    public GameObject cluster;

    public Camera clusterCamera;
    // Start is called before the first frame update
    public void enableAll()
    {
        enablePercentage();
        GetComponent<CanvasGroupFade>().enabled = true;
        gameObject.SetActive(true);
    }

    public void ResetFills()
    {
        line.ResetFills();
        percentage.ResetFills();
        foreach (PlanetEnabler planete in planets)
            planete.ResetFills();
    }

    public void enablePercentage()
    {
        line.enabled = true;
        percentage.enabled = true;
    }

    public void FocusOnPlanet(int i)
    {
        LerpToWhenEnabled lerp = clusterCamera.GetComponent<LerpToWhenEnabled>();
        CameraOrthoPerspLerp ortho = clusterCamera.GetComponent<CameraOrthoPerspLerp>();

        lerp.setToTransform(cluster.transform.GetChild(i));
        lerp.setOffsetZTo(-cluster.transform.GetChild(i).localScale.x);
        ResetFills();

        planetInfo.enabled = true;
        lerp.enabled = true;
        ortho.enabled = true;
        cluster.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
    }

    public void FocusBlackPlanet()
    {
        LerpToWhenEnabled lerp = clusterCamera.GetComponent<LerpToWhenEnabled>();
        CameraOrthoPerspLerp ortho = clusterCamera.GetComponent<CameraOrthoPerspLerp>();
        enablePercentage();

        planetInfo.gameObject.SetActive(true);
        planetInfo.enabled = true;
        lerp.enabled = true;
        ortho.enabled = true;
        foreach(Transform planet in cluster.transform)
        {
            if(planet.childCount > 0)
                planet.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void CloseCluster()
    {
        ResetFills();
        GetComponent<CanvasGroupFade>().enabled = true;
        cluster.gameObject.SetActive(false);
    }
}
