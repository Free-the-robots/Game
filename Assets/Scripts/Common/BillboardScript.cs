using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public Transform faceSecondary = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(faceSecondary == null)
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        else
            transform.rotation = Quaternion.LookRotation((transform.position - faceSecondary.position).normalized, Vector3.up);
    }
}
