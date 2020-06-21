using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LerpAlphaWhenEnabled : MonoBehaviour
{
    float t = 0f;
    public float time = 1f;
    public float minmax = 1f;
    Image imageSprite;

    public bool flip = false;

    public UnityEvent Response;
    // Start is called before the first frame update
    void OnEnable()
    {
        imageSprite = GetComponent<Image>();
        if(t < time)
        {
            if (flip)
            {
                imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, 1f);
            }
            else
                imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, 0);
        }
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (flip)
        {
            imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, Mathf.SmoothStep(1f, minmax, t / time));
        }
        else
            imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, Mathf.SmoothStep(0f, minmax, t / time));

        if (t > time)
        {
            Response.Invoke();
            imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, minmax);
            enabled = false;
        }
    }
}
