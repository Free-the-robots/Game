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
    public UnityEvent ResponseWhenFinishedBegin;
    public UnityEvent ResponseWhenFinishedEnd;

    void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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

            ResponseWhenFinishedEnd.Invoke();
            enabled = false;
            gameObject.SetActive(false);
        }
        else if(canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 1;
            if (flipWhenFinished)
                flip = !flip;

            ResponseWhenFinishedBegin.Invoke();
            enabled = false;
        }
    }

    public void ResetFills()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
