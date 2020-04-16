using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShootBehaviour
{
    public abstract class ShootingBehaviour
    {
        public abstract void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon, ref float t);
    }
    public class Standard : ShootingBehaviour
    {
        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon, ref float t)
        {
            if (t > 1F / attributes.freq)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
                t = 0f;
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

        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon, ref float t)
        {
            animator.SetBool("play",true);
            if (t > 1F / attributes.freq)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
                t = 0f;
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

        public override void ShootBehaviour(Transform transform, SpaceshipData attributes, List<Weapon.Turret> weapon, ref float t)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            if (t > 1F / attributes.freq)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
                t = 0f;
            }
        }
    }
}
