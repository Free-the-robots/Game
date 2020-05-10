using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //trying to single select
            RaycastHit hit;
            int layerMask = 1 << 31;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.collider.GetComponent<LevelSelectorRaycast>().levelName);
                SceneManager.LoadScene(hit.collider.GetComponent<LevelSelectorRaycast>().levelName);
            }
        }
    }
}
