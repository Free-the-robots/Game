using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsystemdialog : MonoBehaviour
{
    private string kikoo = "test";
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.EnableDialogSystem(kikoo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
