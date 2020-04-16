using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISpaceship : Spaceship
{
    public enum TriggerVision { NONE, SPHERE, CHILDTRIGGER };
    public TriggerVision style;
    public float triggerSize = 10f;

    public GameEvent ennemyDead;

    protected NavMeshAgent agent = null;
    protected Transform target;

    protected bool triggered = false;

    protected override void Setup()
    {
        base.Setup();

        spaceshipData = spaceshipData.Clone();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Death()
    {
        ennemyDead.Raise();
        GameObject.Destroy(this.gameObject);
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
}
