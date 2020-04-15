using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWhenEnabled : MonoBehaviour
{
    Vector3 from;
    public Vector3 toOffset;
    public float time = 1f;
    // Start is called before the first frame update
    void OnEnable()
    {
        from = transform.position;
    }

    float t = 0f;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t < 1f)
        {
            transform.position = Vector3.Lerp(from, from+toOffset, t / time);
        }
        else
        {
            enabled = false;
        }
    }
}
