using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileGuided", menuName = "Projectiles/Guided", order = 1)]
    public class ProjectileGuidedData : ProjectileData
    {
        public float forceAmount = 1f;
        public Transform target = null;
    }
}
