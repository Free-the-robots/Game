using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class TurretMultiple : Turret
    {

        private void Start()
        {
        }

        public override void Fire()
        {
            if (freqT > 1 / projectileData.frequency)
            {
                string parentTag = this.GetComponentInParent<Spaceship>().tag;
                int layer = 15;
                if (parentTag.Equals("Player"))
                    layer = 16;

                if (projectileData is Projectiles.ProjectileMultipleStandard)
                {
                    for (int i = 0; i < transform.childCount; ++i)
                        ParticlePooling.Instance.instantiate(parentTag, transform.GetChild(i), projectileData, layer);
                }
                else
                {
                    ParticlePooling.Instance.instantiate(parentTag, transform, projectileData, layer);
                }

                freqT = 0f;
            }
        }
    }
}
