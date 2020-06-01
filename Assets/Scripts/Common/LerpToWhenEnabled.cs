using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LerpToWhenEnabled : MonoBehaviour
{
    public Transform from;
    public Transform to;
    private Vector3 fromPosition;
    public Vector3 fromOffset = Vector3.zero;
    private Quaternion fromQuaternion;

    private Vector3 toPosition;
    public Vector3 toOffset = Vector3.zero;
    private Quaternion toQuaternion;

    public float time = 1f;

    public bool flip = false;
    public bool flipWhenFinished = true;

    public UnityEvent ResponseWhenFinishedBegin;
    public UnityEvent ResponseWhenFinishedEnd;

    public bool lerpRotate = true;
    public bool lerpPosition = true;

    private bool animating = false;

    float t = 0f;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (from == null)
        {
            fromPosition = transform.position + fromOffset;
            fromQuaternion = transform.rotation;
        }
        else
        {
            if (flip)
            {
                fromPosition = to.position + toOffset;
                fromQuaternion = to.rotation;
                toPosition = from.position + fromOffset;
                toQuaternion = from.rotation;
            }
            else
            {
                fromPosition = from.position + fromOffset;
                fromQuaternion = from.rotation;
                toPosition = to.position + toOffset;
                toQuaternion = to.rotation;
            }
        }

        t = 0f;
        animating = true;
    }
    public void enable()
    {
        if (enabled)
        {
            finishedAnimation();
            OnEnable();
        }
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            if (t < time)
            {
                t += Time.deltaTime;
                float tLerp = t * t * (3 - 2 * t);
                if (lerpPosition)
                    transform.position = Vector3.Lerp(fromPosition, toPosition, tLerp / time);
                if (lerpRotate)
                    transform.rotation = Quaternion.Lerp(fromQuaternion, toQuaternion, tLerp / time);
            }
            else
            {
                if (!flip)
                    ResponseWhenFinishedEnd.Invoke();
                else
                    ResponseWhenFinishedBegin.Invoke();
                finishedAnimation();
                enabled = false;
            }
        }
    }

    private void finishedAnimation()
    {
        if (flipWhenFinished)
            flip = !flip;
        if (lerpPosition)
            transform.position = toPosition;
        if (lerpRotate)
            transform.rotation = toQuaternion;
    }

    public void setOffsetZTo(float z)
    {
        toOffset.z = z;
    }

    public void setOffsetZFrom(float z)
    {
        fromOffset.z = z;
    }

    public void setToTransform(Transform toTransform)
    {
        to = toTransform;
    }

    public void setFromTransform(Transform fromTransform)
    {
        from = fromTransform;
    }
}
