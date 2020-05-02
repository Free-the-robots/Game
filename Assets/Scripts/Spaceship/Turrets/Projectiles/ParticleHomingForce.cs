using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleHomingForce : ParticleForce
    {
        public Transform target;

        protected override void FixedUpdate()
        {
            force = (target.position - transform.position).normalized * ((ProjectileGuidedData)data).forceAmount;
            apply(new Vector3(0f, 0f, 1f));
        }
    }
}
