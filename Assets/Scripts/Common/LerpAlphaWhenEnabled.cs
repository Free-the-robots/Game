using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LerpAlphaWhenEnabled : MonoBehaviour
{
    float t = 0f;
    public float time = 1f;
    Image imageSprite;

    public UnityEvent Response;
    // Start is called before the first frame update
    void OnEnable()
    {
        imageSprite = GetComponent<Image>();
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        imageSprite.color = new Color(imageSprite.color.r, imageSprite.color.g, imageSprite.color.b, t / time);
        if(t > time)
        {
            Response.Invoke();
            enabled = false;
        }
    }
}
