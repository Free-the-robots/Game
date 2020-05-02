using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleForce : Particle
    {
        protected Vector3 force = new Vector3(0f, 0f, 1f);

        protected override void apply(Vector3 vel)
        {
            body.velocity = transform.TransformDirection((vel+force).normalized * data.velocity);

            if (t > data.lifeTime)
            {
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
