using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "SData", menuName = "Spaceship/SpaceshipData", order = 1)]
public class SpaceshipData : ScriptableObject
{
    public int id;
    public string shipName;
    public string description;
    public float life;
    public float lifeMax;

    public float speed;

    public float damage;
    public float shield;
    public float armor;
    public int rarity;

    public int modifiableTurretCount = 0;

    public SpaceshipData Clone()
    {
        SpaceshipData res = ScriptableObject.CreateInstance<SpaceshipData>();
        res.life = life;
        res.lifeMax = lifeMax;
        res.speed = speed;
        res.shipName = shipName;
        res.damage = damage;
        res.shield = shield;
        res.armor = armor;
        res.rarity = rarity;
        res.description = description;
        res.modifiableTurretCount = modifiableTurretCount;
        return res;
    }
}
