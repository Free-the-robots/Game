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
            EXPLOSIF
        }

        public delegate void ActionObstacle(GameObject that, Collision other);
        public static Dictionary<SPECIAL, ActionObstacle> obstacleBehaviour = new Dictionary<SPECIAL, ActionObstacle>()
        {
            { SPECIAL.NONE,obstacleCommon},
            { SPECIAL.RICOCHET,obstacleRicochet},
            { SPECIAL.EXPLOSIF,obstacleCommon }
        };



        public delegate void ActionTrigger(GameObject that, Collider other);
        public static Dictionary<SPECIAL, ActionTrigger> enemyTriggerBehaviour = new Dictionary<SPECIAL, ActionTrigger>()
        {
            { SPECIAL.NONE,triggerCommon},
            { SPECIAL.RICOCHET,triggerCommon },
            { SPECIAL.EXPLOSIF,triggerCommon }
        };

        public static void obstacleCommon(GameObject that, Collision other)
        {

        }

        public static void obstacleRicochet(GameObject that, Collision other)
        {
            that.transform.forward = Vector3.Reflect(that.transform.forward, other.GetContact(0).normal);
        }

        public static void triggerCommon(GameObject that, Collider other)
        {

        }
    }
}
