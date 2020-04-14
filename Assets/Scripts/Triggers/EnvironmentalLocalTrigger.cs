using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalLocalTrigger : MonoBehaviour
{
    [TagSelector]
    public string TagFilter = "";
    public UnityEvent Response;

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(TagFilter))
        {
            if (TagFilter.Equals(other.tag))
                Response.Invoke();
        }
        else
        {
            Response.Invoke();
        }
    }
}
