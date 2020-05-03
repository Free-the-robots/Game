using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class Laser : Particle
    {
        protected override void apply(Vector3 vel)
        {
            float life = 0f;
            float dt = 0.05f;
            Vector3 pos = Vector3.zero;
            while (life < data.lifeTime)
            {
                Debug.DrawLine(transform.TransformPoint(pos), transform.TransformPoint(pos + vel.normalized * data.velocity * dt), Color.white, 0f);
                pos += vel.normalized * data.velocity * dt;
                life += dt;
            }
            //Debug.DrawLine(Vector3.zero, new Vector3(10f, 0f, 10f), Color.white, 0f);
            //body.velocity = transform.TransformDirection(vel.normalized);
            if (t > ((LaserEvolutiveData)data).laserLifeTime)
            {
                t = 0f;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}