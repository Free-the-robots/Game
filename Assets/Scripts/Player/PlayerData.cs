using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "PData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float life;
    public float lifeMax;

    public float speed;

    public float freq;

    public PlayerData Clone()
    {
        PlayerData res = ScriptableObject.CreateInstance<PlayerData>();
        res.life = life;
        res.lifeMax = lifeMax;
        res.speed = speed;
        res.freq = freq;
        return res;
    }
}
