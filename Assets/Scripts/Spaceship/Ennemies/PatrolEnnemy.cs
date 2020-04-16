using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnnemy : EnnemyAISpaceship
{
    public List<Transform> destinations = new List<Transform>();

    int actualDest = 0;
    public float waitTime = 5f;

    private bool patrol = true;
    private float prevStopDist = 0f;

    protected override void Setup()
    {
        base.Setup();

        agent.SetDestination(destinations[actualDest].position);

        prevStopDist = agent.stoppingDistance;
        agent.stoppingDistance = 0f;
    }

    float t = 0f;
    float shootingT = 0f;
    protected override void Behaviour()
    {
        base.Behaviour();


        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0 && patrol == true)
        {
            t += Time.deltaTime;
            if(t > waitTime)
            {
                t = 0f;
                actualDest = (actualDest + 1) % destinations.Count;
                agent.SetDestination(destinations[actualDest].position);
            }
        }

        if (CheckTrigger() || !patrol)
        {
            if (patrol)
            {
                t = 0f;
                agent.SetDestination(target.position);
                agent.stoppingDistance = prevStopDist;
            }

            patrol = false;
            t += Time.deltaTime;
            if(t > 0.2f)
            {
                agent.SetDestination(target.position);
                t = 0f;
            }
            shootingT += Time.deltaTime;
            ShootingBehaviour(ref shootingT);
        }
    }

    protected override void FixedBehaviour()
    {
        base.FixedBehaviour();
    }
}
