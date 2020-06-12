using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LerpAlphaWhenEnabled : MonoBehaviour
{
    float t = 0f;
    public float time = 1f;
    public float max = 1f;
    Image imageSprite;

    public UnityEvent Response;
    // Start is called before the first frame update
    void OnEnable()
    {
        imageSprite = GetComponent<Image>();
        if(t < time)
        {
            imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, 0);
        }
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, Mathf.SmoothStep(0f, 1f, t / time * max));
        if(t > time)
        {
            Response.Invoke();
            enabled = false;
        }
    }
}
