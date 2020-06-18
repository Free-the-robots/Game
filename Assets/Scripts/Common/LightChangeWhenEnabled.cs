using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChangeWhenEnabled : MonoBehaviour
{
    public enum MODE{ FLICKER, SINUS}

    public MODE mode;
    private bool animating = false;

    public Vector2 intensityRange = new Vector2(0f, 1f);
    public float freq = 1f;

    private float timeFlicker = 1f;
    private Light lightComp;
    // Start is called before the first frame update
    void OnEnable()
    {
        lightComp = GetComponent<Light>();
        animating = true;
        t = 0f;
    }

    private void OnDisable()
    {
        animating = false;
        t = 0f;
    }

    private float t = 0f;
    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            t += Time.deltaTime;
            switch (mode)
            {
                case MODE.FLICKER:
                    if(t > timeFlicker)
                    {
                        lightComp.intensity = Random.Range(intensityRange.x,intensityRange.y);
                        timeFlicker = Random.Range(0f, 1f/freq);
                        t = 0f;
                    }
                    break;

                case MODE.SINUS:
                    lightComp.intensity = (Mathf.Sin(t * 2f * Mathf.PI * freq)/2f+0.5f) * (intensityRange.y-intensityRange.x) + intensityRange.x;
                    break;
            }
        }
    }
}
