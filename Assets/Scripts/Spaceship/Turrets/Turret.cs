using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Turret : MonoBehaviour
    {
        public Projectiles.ProjectileData projectileData = null;

        protected float freqT = 0f;
        protected float freqW = 0f;
        protected float waitTime = 0f;

        protected bool shooting = true;

        protected virtual void Update()
        {
            freqT += Time.deltaTime;
            freqW += Time.deltaTime;
            waitTime += Time.deltaTime;
        }

        public virtual void Fire()
        {
            if (shooting)
            {
                if (freqT > 1f / projectileData.frequency)
                {
                    string parentTag = this.GetComponentInParent<Spaceship>().tag;
                    int layer = 15;
                    if (parentTag.Equals("Player"))
                        layer = 16;
                    ParticlePooling.Instance.instantiate(parentTag, transform, projectileData, layer);

                    freqT = 0f;
                }
                if (freqW > 1f / projectileData.freqWait)
                {
                    shooting = false;
                    waitTime = 0f;
                }
            }
            else
            {
                if (waitTime > projectileData.waitTime)
                {
                    shooting = true;
                    freqW = 0f;
                }
            }
        }

        public virtual void UpdateRotation()
        {
            //transform.rotation = rotation;
        }
    }
}
