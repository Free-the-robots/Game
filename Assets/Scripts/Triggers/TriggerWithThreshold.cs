using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerWithThreshold : MonoBehaviour
{
    public int threshold = 0;

    public UnityEvent Response;

    void Start()
    {

    }

    public void updateThreshold()
    {
        threshold--;
        if (threshold <= 0)
            Response.Invoke();
    }
}
