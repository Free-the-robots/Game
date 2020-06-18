using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Turret : MonoBehaviour
    {
        public Projectiles.ProjectileData projectileData = null;

        protected float freqT = 0f;
        protected float freqBrustT = 0f;
        protected int bursts = 0;

        protected virtual void Update()
        {
            freqT += Time.deltaTime;
        }

        public virtual void Fire()
        {
            if(freqT > 1f/projectileData.frequency)
            {
                freqBrustT += Time.deltaTime;
                if (freqBrustT > 1f / projectileData.burstFrequency)
                {
                    bursts++;
                    if (bursts >= projectileData.bursts)
                    {
                        bursts = 0;
                        freqT = 0f;
                    }

                    string parentTag = this.GetComponentInParent<Spaceship>().tag;
                    int layer = 15;
                    if (parentTag.Equals("Player"))
                        layer = 16;
                    ParticlePooling.Instance.instantiate(parentTag, transform, projectileData, layer);

                    freqBrustT = 0f;
                }
            }
        }

        public virtual void UpdateRotation()
        {
            //transform.rotation = rotation;
        }
    }
}
