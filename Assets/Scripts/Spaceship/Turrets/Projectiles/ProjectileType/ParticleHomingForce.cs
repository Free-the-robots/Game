using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleHomingForce : ParticleForce
    {
        protected override void FixedUpdate()
        {
            Vector3 enemyPos = ((ProjectileGuidedData)data).target.position;
            force = (transform.InverseTransformPoint(enemyPos) - transform.localPosition).normalized * ((ProjectileGuidedData)data).forceAmount;
            apply(new Vector3(0f, 0f, 1f));
        }
    }
}
