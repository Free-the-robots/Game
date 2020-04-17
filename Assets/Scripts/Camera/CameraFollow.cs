using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private  float r = 5f;

    [SerializeField]
    private Vector3 offsetPosition;

    const float ph = 0.7853981634f; // 45d
    const float th = 0f; //-0.7853981634f; // 45d
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position + r * new Vector3(Mathf.Sin(ph), Mathf.Cos(ph), 0f);//new Vector3(Mathf.Cos(th)*Mathf.Sin(ph), Mathf.Cos(ph), Mathf.Sin(th) * Mathf.Sin(ph));

        transform.position = target.TransformPoint(offsetPosition);
        transform.LookAt(target);
    }
}
