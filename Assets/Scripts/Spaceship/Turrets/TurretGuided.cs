using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class TurretGuided : Turret
    {
        public Transform target;

        public override void UpdateRotation(Quaternion rotation)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}
