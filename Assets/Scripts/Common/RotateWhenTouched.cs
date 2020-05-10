using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWhenTouched : MonoBehaviour
{
    float sensibility = 0.3f;
    bool touched = false;
    GameObject planet;

    Vector3 mousePos;
    Quaternion planetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!touched)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //trying to single select
                RaycastHit hit;
                int layerMask = 1 << 30;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    touched = true;
                    planet = hit.collider.gameObject;
                    planet.GetComponent<RotateWhenEnabled>().enabled = false;

                    planetRotation = planet.transform.rotation;

                    mousePos = Input.mousePosition;
                }
            }
            else
            {
                Vector3 posDiff = (Input.mousePosition - mousePos)*sensibility;
                mousePos = Input.mousePosition;

                planet.transform.Rotate(planet.transform.up, -posDiff.x, Space.World);
                planet.transform.Rotate(Camera.main.transform.right, posDiff.y, Space.World);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            touched = false;
            if(planet != null)
                planet.GetComponent<RotateWhenEnabled>().enabled = true;
        }
    }
}
