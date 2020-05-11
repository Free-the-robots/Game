using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CanvasGroupFade : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float time = 1f;
    public bool flip = false;
    public bool flipWhenFinished = true;
    private short step = 1;

    public bool disableGameObjectWhenFinished = true;
    public bool activateReponseWhenFinished = false;
    public bool InvokeWhenFinished { get { return activateReponseWhenFinished; } set { activateReponseWhenFinished = value; } }
    public UnityEvent ResponseWhenFinished;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void OnEnable()
    {
        if (flip)
            step = 1;
        else
            step = -1;
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.alpha += step*Time.deltaTime/time;
        if(canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            if (flipWhenFinished)
                flip = !flip;
            if(activateReponseWhenFinished)
                ResponseWhenFinished.Invoke();
            enabled = false;
            gameObject.SetActive(false);
        }
        else if(canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 1;
            if (flipWhenFinished)
                flip = !flip;
            //if(activateReponseWhenFinished)
            //    ResponseWhenFinished.Invoke();
            enabled = false;
        }
    }
}
