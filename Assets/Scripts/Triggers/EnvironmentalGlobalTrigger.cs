using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalGlobalTrigger : MonoBehaviour
{
    public GameEvent eventObj;

    [TagSelector]
    public string TagFilter = "";

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(TagFilter))
        {
            if (TagFilter.Equals(other.tag))
                eventObj.Raise();
        }
        else
        {
            eventObj.Raise();
        }
    }
}
