﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillShip3D : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        AssetDataManager assetManager = AssetDataManager.Instance;
        foreach(GameObject ship in assetManager.spaceshipObject)
        {
            GameObject gb = GameObject.Instantiate(ship);
            gb.GetComponent<PlayerSpaceship>().enabled = false;
            foreach (Collider coll in gb.GetComponentsInChildren<Collider>())
                coll.enabled = false;
            gb.transform.SetParent(transform,false);
            gb.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach(Transform trans in transform)
        {
            GameObject.Destroy(trans.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}