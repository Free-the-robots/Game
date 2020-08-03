using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField]
    private GameObject DialogSystem;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Display the dialog system with the name of the dialog data JSON
    /// </summary>
    /// <param name="iName">Name of the dialog data JSON</param>
    public void EnableDialogSystem(string iName, Action<bool> IfunctionAtTheEndOfDialog = null)
    {
        DialogSystem.GetComponent<DialogSystem>().InstantiateDialogSystem(DialogSystem, iName, IfunctionAtTheEndOfDialog);

    }

}
