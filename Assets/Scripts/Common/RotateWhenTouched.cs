using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateWhenTouched : MonoBehaviour
{
    float sensibility = 0.3f;
    bool touched = false;
    GameObject planet;

    Vector3 mousePos;
    Quaternion planetRotation;

    public Camera cameraTexture;
    public RawImage renderTexture;

    private RectTransform rectTexture;
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
                if (cameraTexture)
                {
                    RectTransform rect1 = renderTexture.rectTransform;
                    Vector3 pos = Input.mousePosition;
                    pos.Scale(new Vector3(1f / Screen.width, 1f / Screen.height, 1f));
                    pos.Scale(new Vector3(renderTexture.mainTexture.width, renderTexture.mainTexture.height, 1f));
                    ray = cameraTexture.ScreenPointToRay(pos); //trying to single select
                }
                RaycastHit hit;

                int layerMask = 1 << 30;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    touched = true;
                    planet = hit.collider.gameObject;
                    if(planet != null && planet.GetComponent<RotateWhenEnabled>() != null)
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
                if(cameraTexture)
                    planet.transform.Rotate(cameraTexture.transform.right, posDiff.y, Space.World);
                else
                    planet.transform.Rotate(Camera.main.transform.right, posDiff.y, Space.World);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            touched = false;
            if(planet != null && planet.GetComponent<RotateWhenEnabled>() != null)
                planet.GetComponent<RotateWhenEnabled>().enabled = true;
        }
    }
}
