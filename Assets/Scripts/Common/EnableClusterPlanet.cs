using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClusterPlanet : MonoBehaviour
{
    public int id = 0;
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
        line.maxPercentile = UserData.UserDataManager.Instance.userData.clusters[id].percentage();
        line.enabled = true;
        percentage.maxPercentile = line.maxPercentile;
        percentage.enabled = true;
    }

    public void FocusOnPlanet(int i)
    {
        LerpToWhenEnabled lerp = clusterCamera.GetComponent<LerpToWhenEnabled>();
        CameraOrthoPerspLerp ortho = clusterCamera.GetComponent<CameraOrthoPerspLerp>();

        lerp.setToTransform(cluster.transform.GetChild(i));
        lerp.setOffsetZTo(-cluster.transform.GetChild(i).GetChild(0).localScale.x*2.5f);
        ResetFills();

        planetInfo.enable();
        lerp.enable();
        ortho.enable();
        cluster.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        cluster.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
    }

    public void FocusBackPlanet()
    {
        LerpToWhenEnabled lerp = clusterCamera.GetComponent<LerpToWhenEnabled>();
        CameraOrthoPerspLerp ortho = clusterCamera.GetComponent<CameraOrthoPerspLerp>();
        enablePercentage();

        planetInfo.gameObject.SetActive(true);
        planetInfo.enable();
        lerp.enable();
        ortho.enable();
        foreach (Transform planet in cluster.transform)
        {
            if(planet.childCount > 0)
                planet.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void CloseCluster()
    {
        ResetFills();
        GetComponent<CanvasGroupFade>().enabled = true;
        cluster.gameObject.SetActive(false);
    }
}
