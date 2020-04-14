using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyAISpaceship : Spaceship
{
    protected NavMeshAgent agent;

    protected override void Setup()
    {
        base.Setup();

        agent = GetComponent<NavMeshAgent>();
    }
}
