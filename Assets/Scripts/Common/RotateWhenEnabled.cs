using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWhenEnabled : MonoBehaviour
{
    Quaternion from;
    public Vector3 to;
    public float time = 1f;
    // Start is called before the first frame update
    void OnEnable()
    {
        from = transform.rotation;
    }

    float t = 0f;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t < 1f)
        {
            transform.rotation = Quaternion.Lerp(from, Quaternion.Euler(to), t/time);
        }
        else
        {
            enabled = false;
        }
    }
}
