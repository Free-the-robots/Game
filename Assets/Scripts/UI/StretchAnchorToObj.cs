using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchAnchorToObj : MonoBehaviour
{
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        //GetComponent<RectTransform>().pivot = rectTransform.pivot;
        GetComponent<RectTransform>().sizeDelta = new Vector2(0f, rectTransform.rect.height);
        //GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        //GetComponent<RectTransform>().anchorMax = rectTransform.anchorMax;
        //GetComponent<RectTransform>().anchorMin = rectTransform.anchorMin;
    }
}
