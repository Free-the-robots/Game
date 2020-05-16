using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public bool flipWhenFinished = true;

    public UnityEvent ResponseWhenFinishedBegin;
    public UnityEvent ResponseWhenFinishedEnd;

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
        if (t < time)
        {
            t += Time.deltaTime;
            float tLerp = t * t * (3 - 2 * t);
            transform.position = Vector3.Lerp(fromPosition, toPosition, tLerp / time);
            transform.rotation = Quaternion.Lerp(fromQuaternion, toQuaternion, tLerp / time);
        }
        else
        {
            if(!flip)
                ResponseWhenFinishedEnd.Invoke();
            else
                ResponseWhenFinishedBegin.Invoke();
            if (flipWhenFinished)
                flip = !flip;
            transform.position = toPosition;
            transform.rotation = toQuaternion;
            enabled = false;
        }
    }
}
