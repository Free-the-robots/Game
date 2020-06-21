using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public static LoadingManager Instance { get { return instance; } }

    public RectTransform rectLoadingParent;
    public RectTransform rectLoading;
    public Text message;

    private float maxPercent = 0f;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        gameObject.SetActive(false);
    }

    public void setPercent(float p)
    {
        float maxWidth = rectLoadingParent.rect.width;
        rectLoading.sizeDelta = new Vector2(p / maxPercent * maxWidth, rectLoading.sizeDelta.y);
    }

    public void enableLoading(string mes)
    {
        message.text = mes;
        gameObject.SetActive(true);
    }

    public void disableLoading()
    {
        gameObject.SetActive(false);
    }

    public void resetLoading()
    {
        rectLoading.sizeDelta = new Vector2(0f, rectLoading.sizeDelta.y);

        maxPercent = 0f;
    }

    public void resetLoading(float max)
    {
        rectLoading.sizeDelta = new Vector2(0f, rectLoading.sizeDelta.y);

        maxPercent = max;
    }

    public void setMaxPercent(float max)
    {
        maxPercent = max;
    }
}
