using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWhenEnabled : MonoBehaviour
{
    public enum ROTATE_TYPE { CLOCKWISE, ANTICLOCKWISE, CLOSEST}
    Quaternion from;
    public ROTATE_TYPE rotate_type = ROTATE_TYPE.CLOSEST;
    public Vector3 to;
    public float time = 1f;
    public bool loop = false;
    public bool resetLoop = false;

    private Vector3 stepLoop;
    // Start is called before the first frame update
    void OnEnable()
    {
        from = transform.rotation;
        stepLoop = (to);
    }

    float t = 0f;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t < time)
        {
            switch (rotate_type)
            {
                case ROTATE_TYPE.ANTICLOCKWISE:
                    transform.Rotate(stepLoop / time * Time.deltaTime, Space.Self);
                    break;
                case ROTATE_TYPE.CLOCKWISE:
                    transform.Rotate(-stepLoop / time * Time.deltaTime, Space.Self);
                    break;
                case ROTATE_TYPE.CLOSEST:
                    transform.rotation = Quaternion.Lerp(from, Quaternion.Euler(to), t / time);
                    break;
            }
        }
        else
        {
            if (loop)
            {
                t = 0f;
                if (resetLoop)
                    transform.rotation = from;
            }
            else
                enabled = false;
        }
    }
}
