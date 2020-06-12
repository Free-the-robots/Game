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
    private float steps = 0f;

    public float maxPercentile = 1.0f;

    public List<UnityEvent> ActivatePercentage = new List<UnityEvent>();
    public List<bool> activated = new List<bool>();
    public UnityEvent Response;
    // Start is called before the first frame update
    void OnEnable()
    {
        t = 0f;
        parentRect = parent.GetComponent<RectTransform>();
        currentRect = GetComponent<RectTransform>();

        activated.Clear();
        for (int i = 0; i < ActivatePercentage.Count; i++)
            activated.Add(false);

        if (width)
        {
            w = parentRect.rect.width;
            steps = w / (ActivatePercentage.Count-1);
        }
        else
            w = currentRect.sizeDelta.x;

        if (height) {
            h = parentRect.rect.height;
            steps = h / (ActivatePercentage.Count - 1);
        }
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
            tw = Mathf.SmoothStep(0f,1f,t / time * maxPercentile);
        if (height)
            th = Mathf.SmoothStep(0f, 1f, t / time * maxPercentile);

        if(ActivatePercentage.Count > 0)
        {
            for (int i = ActivatePercentage.Count; i >= 0; i--)
            {
                if (currentRect.sizeDelta.x >= (i-0.1) * steps && !activated[i])
                {
                    ActivatePercentage[i].Invoke();
                    activated[i] = true;
                    break;
                }
            }
        }

        if (t > time)
        {
            tw = 1f;
            th = 1f;
            if (width)
            {
                currentRect.sizeDelta = new Vector2(w * maxPercentile, h);
                tw = maxPercentile;
            }
            if (height)
            {
                currentRect.sizeDelta = new Vector2(w, h * maxPercentile);
                th = maxPercentile;
            }

            Response.Invoke();

            enabled = false;
        }
        currentRect.sizeDelta = new Vector2(w * tw, h * th);
    }

    private void OnDisable()
    {
        
    }

    public void ResetFills()
    {
        if (width)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(0f, h);
            t = 0f;
        }
        if (height)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(w, 0f);
            t = 0f;
        }
    }
}
