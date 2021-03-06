﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISpaceship : Spaceship
{
    public enum TriggerVision { NONE, SPHERE, CHILDTRIGGER };
    public TriggerVision style;
    public float triggerSize = 10f;

    public enum AIBehaviour { NONE, ANIMATION, FOLLOWPLAYER };
    public AIBehaviour aiBehaviour;

    public GameEvent ennemyDead;

    protected NavMeshAgent agent = null;
    protected Transform target;

    protected bool triggered = false;

    protected ShootBehaviour.ShootingBehaviour shootingBehaviour = null;

    protected override void Setup()
    {
        base.Setup();

        spaceshipData = spaceshipData.Clone();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        switch (aiBehaviour)
        {
            case AIBehaviour.ANIMATION:
                shootingBehaviour = new ShootBehaviour.AnimationBehaviour(GetComponent<Animator>());
                break;
            case AIBehaviour.FOLLOWPLAYER:
                shootingBehaviour = new ShootBehaviour.FollowBehaviour(target);
                break;
            default:
                break;
        }
    }

    public override void Death()
    {
        ennemyDead.Raise();
        base.Death();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        switch (style)
        {
            case TriggerVision.SPHERE:
                Gizmos.DrawWireSphere(transform.position, triggerSize);
                break;
            default:
                break;
        }
    }

    public virtual void Triggered()
    {
        triggered = true;
    }

    protected virtual bool CheckTrigger()
    {
        if (style == TriggerVision.SPHERE)
            return (Vector3.Distance(target.position, transform.position) < triggerSize);
        else
            return (triggered);
    }

    protected virtual void ShootingBehaviour(ref float t)
    {
        if(shootingBehaviour == null)
        {
            if (t > 1F / spaceshipData.freq)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
                t = 0f;
            }
        }
        else
        {
            shootingBehaviour.ShootBehaviour(transform, spaceshipData, weapon, ref t);
        }
    }
}
