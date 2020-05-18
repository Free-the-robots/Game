using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class StatsUpdate : MonoBehaviour
{
    public UIPolygons polygons;
    float[] prevvalues = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
    float[] stats = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
    float[] values = new float[7] { 0f, 0f, 0f, 0f, 0f, 0f, 0f };

    public float time = 1f;
    bool change = false;

    float t = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            t += Time.deltaTime;
            float tTime = t * t * (3 - 2 * t);
            for (int i = 0; i< 6; i++)
            {
                values[i] = Mathf.Lerp(prevvalues[i], stats[i], tTime / time);
            }
            values[6] = values[0];
            polygons.DrawPolygon(6, values, 90f);
            polygons.SetVerticesDirty();

            if (t > time)
            {
                t = 0f;
                change = false;
            }
        }
    }

    public void updateStats(float[] statsVal)
    {
        for (int i = 0; i < 6; i++)
            prevvalues[i] = values[i];
        stats = statsVal;
        change = true;
    }

    public void testStats()
    {
        float[] val = new float[6] { Random.value, Random.value, Random.value, Random.value, Random.value, Random.value };
        updateStats(val);
    }
}
