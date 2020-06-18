using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShootBehaviour
{
    public abstract class ShootingBehaviour
    {
        public abstract void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon);
    }
    public class Standard : ShootingBehaviour
    {
        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon)
        {
            for (int i = 0; i < weapon.Count; ++i)
            {
                weapon[i].Fire();
            }
        }
    }
    public class AnimationBehaviour : ShootingBehaviour
    {
        public Animator animator;
        public AnimationBehaviour(Animator animator)
        {
            this.animator = animator;
        }

        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon)
        {
            animator.SetBool("play",true);
            for (int i = 0; i < weapon.Count; ++i)
            {
                weapon[i].Fire();
            }
        }
    }
    public class FollowBehaviour : ShootingBehaviour
    {
        public Transform target;
        public FollowBehaviour(Transform target)
        {
            this.target = target;
        }

        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            if (dir.magnitude < 8f)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
            }
        }
    }
}
