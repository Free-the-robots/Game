using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEnabler : MonoBehaviour
{
    public FillUIParent line;
    public FillUIParent planet;
    public GameObject circle;

    public void ResetFills()
    {
        line.ResetFills();
        planet.ResetFills();
        circle.SetActive(false);
    }
}
