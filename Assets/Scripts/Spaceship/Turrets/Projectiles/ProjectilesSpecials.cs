using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Projectiles
{
    public class ProjectilesSpecials : MonoBehaviour
    {
        public enum SPECIAL
        {
            NONE,
            RICOCHET,
            EXPLOSIF,
            SUSTAIN
        }

        public delegate void ActionObstacle(GameObject that, Collision other);
        public static Dictionary<SPECIAL, ActionObstacle> obstacleBehaviour = new Dictionary<SPECIAL, ActionObstacle>()
        {
            { SPECIAL.NONE,obstacleCommon},
            { SPECIAL.RICOCHET,obstacleRicochet},
            { SPECIAL.EXPLOSIF,obstacleCommon },
            { SPECIAL.SUSTAIN,obstacleCommon }
        };



        public delegate void ActionTrigger(GameObject that, Collider other);
        public static Dictionary<SPECIAL, ActionTrigger> enemyTriggerBehaviour = new Dictionary<SPECIAL, ActionTrigger>()
        {
            { SPECIAL.NONE,triggerCommon},
            { SPECIAL.RICOCHET,triggerCommon },
            { SPECIAL.EXPLOSIF,triggerExplosif },
            { SPECIAL.SUSTAIN, triggerSustain }
        };

        public static void obstacleCommon(GameObject that, Collision other)
        {
            ParticlePooling.Destroy(that);
        }

        public static void obstacleRicochet(GameObject that, Collision other)
        {
            that.transform.forward = Vector3.Reflect(that.transform.forward, other.GetContact(0).normal);
        }

        public static void triggerCommon(GameObject that, Collider other)
        {
            ParticlePooling.Instance.destroy(that.gameObject);
        }

        public static void triggerSustain(GameObject that, Collider other)
        {
        }

        public static void triggerExplosif(GameObject that, Collider other)
        {
            Particle particle = that.GetComponent<ParticleChooser>().active;
            ParticlePooling.Instance.instantiate(particle.tag, that.transform, particle.data.explosion, that.layer);

            triggerCommon(that, other);
        }
    }
}
