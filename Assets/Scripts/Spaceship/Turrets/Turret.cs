using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Turret : MonoBehaviour
    {
        public Projectiles.ProjectileData projectileData = null;

        protected float freqT = 0f;

        protected virtual void Update()
        {
            freqT += Time.deltaTime;
        }

        public virtual void Fire()
        {
            if(freqT > 1/projectileData.frequency)
            {
                string parentTag = this.GetComponentInParent<Spaceship>().tag;
                int layer = 15;
                if (parentTag.Equals("Player"))
                    layer = 16;
                ParticlePooling.Instance.instantiate(parentTag, transform, projectileData, layer);

                freqT = 0f;
            }
        }

        public virtual void UpdateRotation()
        {
            //transform.rotation = rotation;
        }
    }
}
