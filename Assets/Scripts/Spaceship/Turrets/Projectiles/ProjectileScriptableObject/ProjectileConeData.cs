using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileCone", menuName = "Projectiles/Cone", order = 1)]
    public class ProjectileConeData : ProjectileData
    {
        public float spreadAngle = 40f;
    }
}
