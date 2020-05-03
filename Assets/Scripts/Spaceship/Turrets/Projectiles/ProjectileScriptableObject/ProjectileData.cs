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
}
