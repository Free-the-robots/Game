using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleCone : Particle
    {
        private float angle = 0f;
        protected override void OnEnable()
        {
            base.OnEnable();
            angle = Random.Range(-((ProjectileConeData)data).spreadAngle, ((ProjectileConeData)data).spreadAngle);
        }
        protected override void apply(Vector3 vel)
        {
            body.velocity = transform.TransformDirection(Quaternion.Euler(0, angle, 0) * vel.normalized * data.velocity);

            if (t > data.lifeTime)
            {
                t = 0f;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
