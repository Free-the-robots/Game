using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class Laser : Particle
    {
        LineRenderer lineRenderer;

        protected override void OnEnable()
        {
            base.OnEnable();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = true;

            lineRenderer.positionCount = (int)(data.lifeTime / Time.fixedDeltaTime);
        }
        protected override void apply(Vector3 vel)
        {
            float life = 0f;
            float dt = Time.fixedDeltaTime;
            Vector3 pos = Vector3.zero;

            List<Vector3> path = new List<Vector3>(lineRenderer.positionCount);
            while (life < data.lifeTime)
            {
                path.Add(transform.TransformPoint(pos));
                //Debug.DrawLine(transform.TransformPoint(pos), transform.TransformPoint(pos + vel.normalized * data.velocity * dt), Color.white, 0f);
                pos += vel.normalized * data.velocity * dt;
                life += dt;
            }
            lineRenderer.SetPositions(path.ToArray());

            path.Clear();
            path = null;
            //Debug.DrawLine(Vector3.zero, new Vector3(10f, 0f, 10f), Color.white, 0f);
            //body.velocity = transform.TransformDirection(vel.normalized);
            if (t > ((LaserData)data).laserLifeTime)
            {
                t = 0f;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;

                lineRenderer.positionCount = 0;
                lineRenderer.enabled = false;

                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}