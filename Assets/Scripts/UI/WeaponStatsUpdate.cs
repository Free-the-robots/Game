using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsUpdate : MonoBehaviour
{
    RectTransform damageP;
    RectTransform firerateP;
    RectTransform rangeP;

    public RectTransform damage;
    public RectTransform firerate;
    public RectTransform range;

    public float time = 1f;

    private float pdamageV = 0f;
    private float pfirerateV = 0f;
    private float prangeV = 0f;

    private float damageV;
    private float firerateV;
    private float rangeV;

    private bool animating = false;
    private float t = 0f;

    private void OnEnable()
    {
        damageP = damage.transform.parent.GetComponent<RectTransform>();
        firerateP = firerate.transform.parent.GetComponent<RectTransform>();
        rangeP = range.transform.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (animating)
        {
            t += Time.deltaTime;
            float tTime = t * t * (3 - 2 * t);

            float w = damageP.rect.width;
            float h = damageP.rect.height;
            damage.sizeDelta = new Vector2(w * pdamageV * (1 - tTime) + tTime * damageV, h);

            w = firerateP.rect.width;
            h = firerateP.rect.height;
            firerate.sizeDelta = new Vector2(w * pfirerateV * (1 - tTime) + tTime * firerateV, h);

            w = rangeP.rect.width;
            h = rangeP.rect.height;
            range.sizeDelta = new Vector2(w * prangeV * (1 - tTime) + tTime * rangeV, h);


            if (t >= 1f)
            {
                animating = false;
                t = 0f;
            }
        }
        
    }

    public void UpdateStat(float dam, float freq, float life)
    {
        damageV = dam;
        firerateV = freq;
        rangeV = life;

        t = 0f;
        animating = true;
    }
}
