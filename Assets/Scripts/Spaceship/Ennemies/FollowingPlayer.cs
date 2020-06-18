using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayer : EnnemyAISpaceship
{
    protected override void Setup()
    {
        base.Setup();

        agent.SetDestination(target.position);
    }

    float t = 0f;
    protected override void Behaviour()
    {
        base.Behaviour();

        t += Time.deltaTime;
        if (t > 0.2f)
        {
            agent.SetDestination(target.position);
            t = 0f;
        }
        ShootingBehaviour();
    }
}
