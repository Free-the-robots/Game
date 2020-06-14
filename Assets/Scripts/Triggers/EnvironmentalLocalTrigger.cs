using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalLocalTrigger : MonoBehaviour
{
    [TagSelector]
    public string TagFilter = "";
    public UnityEvent Response;

    public EventTransform ResponseFromTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(TagFilter))
        {
            if (TagFilter.Equals(other.tag))
            {
                Response.Invoke();
                ResponseFromTrigger.Invoke(transform);
            }
        }
        else
        {
            Response.Invoke();
            ResponseFromTrigger.Invoke(transform);
        }
    }
}
