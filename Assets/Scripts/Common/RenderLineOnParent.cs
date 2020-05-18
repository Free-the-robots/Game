using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineOnParent : MonoBehaviour
{
    private LineRenderer line;
    private Transform parent;
    private float radius;

    public Transform bottom;

    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        parent = transform.parent;
        radius = parent.localScale.x;

        Vector3 radiusPos = (bottom.position - parent.position).normalized;

        line.positionCount = 2;

        line.SetPosition(0, transform.InverseTransformPoint(bottom.position));
        line.SetPosition(1, transform.InverseTransformPoint(parent.position + radiusPos*radius/2f));
    }
}
