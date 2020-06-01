using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Camera cameraTexture;
    public RawImage renderTexture;
    public int id = 0;
    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

            int layerMask = 1 << 31;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                hit.collider.GetComponent<LevelSelectorRaycast>().clicked();
            }

        }
    }
}
