using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Weapon
{
    public class Abilities
    {
        public virtual void RunAbility(Transform transform)
        {
        }
    }

    public class CircleDeath : Abilities
    {
        public override void RunAbility(Transform transform)
        {
            List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector3.Distance(enemies[i].transform.position, transform.position) < 10f)
                    enemies[i].GetComponent<Spaceship>().loseHealth(1000);
            }
        }
    }
}
