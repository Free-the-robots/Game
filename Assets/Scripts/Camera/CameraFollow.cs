﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private  float r = 5f;

    //[SerializeField]
    //private Vector3 offsetPosition;

    public float ph = 60; // 45d
    public float th = 0f; //-0.7853981634f; // 45d
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(findTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            float phR = Mathf.Deg2Rad * ph;
            float thR = Mathf.Deg2Rad * th;
            transform.position = target.position + r * new Vector3(Mathf.Cos(thR)*Mathf.Sin(phR), Mathf.Cos(phR), Mathf.Sin(thR) * Mathf.Sin(phR));//new Vector3(Mathf.Cos(th)*Mathf.Sin(ph), Mathf.Cos(ph), Mathf.Sin(th) * Mathf.Sin(ph));

            //transform.position = target.TransformPoint(offsetPosition);
            transform.LookAt(target);
        }
    }

    IEnumerator findTarget()
    {
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null;
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
