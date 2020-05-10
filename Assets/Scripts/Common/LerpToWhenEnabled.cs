using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToWhenEnabled : MonoBehaviour
{
    public Transform from;
    public Transform to;
    private Vector3 fromPosition;
    private Quaternion fromQuaternion;

    private Vector3 toPosition;
    private Quaternion toQuaternion;

    public float time = 1f;

    public bool flip = false;

    float t = 0f;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if(from == null)
        {
            fromPosition = transform.position;
            fromQuaternion = transform.rotation;
        }
        else
        {
            if (flip)
            {
                fromPosition = to.position;
                fromQuaternion = to.rotation;
                toPosition = from.position;
                toQuaternion = from.rotation;
            }
            else
            {
                fromPosition = from.position;
                fromQuaternion = from.rotation;
                toPosition = to.position;
                toQuaternion = to.rotation;
            }
        }

        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (t/time < 1f)
        {
            t += Time.deltaTime;
            float tLerp = t * t * (3 - 2 * t);
            transform.position = Vector3.Lerp(fromPosition, toPosition, tLerp / time);
            transform.rotation = Quaternion.Lerp(fromQuaternion, toQuaternion, tLerp / time);
        }
        else
        {
            transform.position = toPosition;
            transform.rotation = toQuaternion;
            enabled = false;
        }
    }
}
