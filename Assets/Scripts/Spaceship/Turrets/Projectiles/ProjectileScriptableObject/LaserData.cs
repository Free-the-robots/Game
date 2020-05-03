using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "Laser", menuName = "Projectiles/Laser", order = 2)]
    public class LaserData : ProjectileData
    {
        public float laserLifeTime = 1f;
    }
}
