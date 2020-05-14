using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FillUIParent : MonoBehaviour
{
    public float time = 1f;
    public Transform parent;
    float t = 0f;
    private RectTransform parentRect;
    private RectTransform currentRect;
    public bool width = true;
    private float w = 0f;
    public bool height = false;
    private float h = 0f;

    public UnityEvent Response;
    // Start is called before the first frame update
    void OnEnable()
    {
        t = 0f;
        parentRect = parent.GetComponent<RectTransform>();
        currentRect = GetComponent<RectTransform>();
        if (width)
            w = parentRect.rect.width;
        else
            w = currentRect.sizeDelta.x;

        if (height)
            h = parentRect.rect.height;
        else
            h = currentRect.sizeDelta.y;


    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        float tw = 1f;
        float th = 1f;
        if (width)
            tw = t / time;
        if (height)
            th = t / time;
        if(t > time)
        {
            currentRect.sizeDelta = new Vector2(w, h);
            tw = 1f;
            th = 1f;

            Response.Invoke();

            enabled = false;
        }
        currentRect.sizeDelta = new Vector2(w * tw, h * th);
    }

    private void OnDisable()
    {
        
    }
}
