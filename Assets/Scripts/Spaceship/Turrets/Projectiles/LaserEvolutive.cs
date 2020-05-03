using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class LaserEvolutive : ParticleEvolutive
    {

        protected override void FixedUpdate()
        {
            List<float> res = ((LaserEvolutiveData)data).behaviour.evaluate(inputs);
            Vector3 vel = (new Vector3(res[1], 0f, res[0])).normalized;

            float life = 0f;
            float dt = 0.01f;
            Vector3 pos = Vector3.zero;
            while (life < data.lifeTime)
            {
                //Scale Independant transform.TransformPoint()
                Matrix4x4 m = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

                //Debug.DrawLine(transform.TransformPoint(pos), transform.TransformPoint(pos + vel.normalized * data.velocity * dt), Color.white, 0f);
                Debug.DrawLine(m.MultiplyPoint3x4(pos), m.MultiplyPoint3x4(pos + (vel + Vector3.forward).normalized * data.velocity * dt), Color.white, 0f);
                pos += (vel + Vector3.forward).normalized * data.velocity * dt;
                pos.y = 0f;
                life += dt;

                //pos = transform.InverseTransformPoint(transform.position - initPos);
                inputs[0] = pos.z;
                inputs[1] = pos.x;
                inputs[2] = Vector3.Distance(Vector3.zero, pos);

                res = ((LaserEvolutiveData)data).behaviour.evaluate(inputs);
                vel.x = res[1];
                vel.y = 0f;
                vel.z = res[0];
                vel.Normalize();
            }
            apply(vel);
        }

        protected override void apply(Vector3 vel)
        {
            //Debug.DrawLine(Vector3.zero, new Vector3(10f, 0f, 10f), Color.white, 0f);
            //body.velocity = transform.TransformDirection(vel.normalized);
            if (t > ((LaserEvolutiveData)data).laserLifeTime)
            {
                t = 0f;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
