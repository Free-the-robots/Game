using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISpaceship : Spaceship
{
    protected NavMeshAgent agent = null;
    public float lookRadius = 10f;
    protected Transform target;

    public GameEvent ennemyDead;

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
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
