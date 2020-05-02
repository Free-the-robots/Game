using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        public Texture2D skin = null;
        public float velocity = 1f;
        public float frequency = 1f;
        public float lifeTime = 1f;
        public float damage = 0f;
    }

    [CreateAssetMenu(fileName = "ProjectileGuided", menuName = "Projectiles/ProjectileGuided", order = 1)]
    public class ProjectileGuidedData : ProjectileData
    {
        public float forceAmount = 1f;
    }

    [CreateAssetMenu(fileName = "Laser", menuName = "Projectiles/Laser", order = 2)]
    public class LaserData : ProjectileData
    {

    }

    [CreateAssetMenu(fileName = "ProjectileEvolutive", menuName = "Projectiles/ProjectileEvolutive", order = 3)]
    public class ProjectileEvolutiveData : ProjectileData
    {
        public NEAT.Person behaviour;
    }
}
