using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "SData", menuName = "Spaceship/SpaceshipData", order = 1)]
public class SpaceshipData : ScriptableObject
{
    public float life;
    public float lifeMax;

    public float speed;

    public float freq;

    public SpaceshipData Clone()
    {
        SpaceshipData res = ScriptableObject.CreateInstance<SpaceshipData>();
        res.life = life;
        res.lifeMax = lifeMax;
        res.speed = speed;
        res.freq = freq;
        return res;
    }
}
