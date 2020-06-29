using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISpaceship : Spaceship
{
    public enum TriggerVision { NONE, SPHERE, CHILDTRIGGER };
    public TriggerVision triggerStyle;
    public float triggerSize = 10f;

    public enum ShooterBehaviour { NONE, ANIMATION, FOLLOWPLAYER };
    public ShooterBehaviour shooterBehaviour;

    public GameEvent enemyDead;

    protected NavMeshAgent agent = null;
    protected Transform target;

    protected bool triggered = false;

    protected ShootBehaviour.ShootingBehaviour shootingBehaviour = null;

    protected MaterialPropertyBlock materialBlock;

    public void Enable()
    {
        enabled = true;
    }
    protected override void Setup()
    {
        base.Setup();

        spaceshipData = spaceshipData.Clone();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        materialBlock = new MaterialPropertyBlock();

        switch (shooterBehaviour)
        {
            case ShooterBehaviour.ANIMATION:
                shootingBehaviour = new ShootBehaviour.AnimationBehaviour(GetComponent<Animator>());
                break;
            case ShooterBehaviour.FOLLOWPLAYER:
                shootingBehaviour = new ShootBehaviour.FollowBehaviour(target);
                break;
            default:
                shootingBehaviour = new ShootBehaviour.Standard();
                break;
        }
    }

    protected override void Behaviour()
    {
        ShootingBehaviour();
    }

    public override void Death()
    {
        enemyDead.Raise();
        base.Death();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        switch (triggerStyle)
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
        if (triggerStyle == TriggerVision.SPHERE)
            return (Vector3.Distance(target.position, transform.position) < triggerSize);
        else
            return (triggered);
    }

    protected virtual void ShootingBehaviour()
    {
        if(shootingBehaviour == null)
        {
            for (int i = 0; i < weapon.Count; ++i)
            {
                weapon[i].Fire();
            }
        }
        else
        {
            shootingBehaviour.ShootBehaviour(transform, spaceshipData, weapon);
        }
    }

    protected override void setColor(Color color)
    {
        rendererComp.GetPropertyBlock(materialBlock, 0);
        // Assign our new value.
        materialBlock.SetColor("_Color", color);
        // Apply the edited values to the renderer.
        rendererComp.SetPropertyBlock(materialBlock, 0);
    }
}
