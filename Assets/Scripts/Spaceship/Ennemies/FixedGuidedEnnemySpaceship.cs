using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedGuidedEnnemySpaceship : EnnemyAISpaceship
{

    protected override void Setup()
    {
        base.Setup();
    }

    protected override void Behaviour()
    {
        base.Behaviour();

        if (Vector3.Distance(target.position, transform.position) < triggerSize)
        {
            ShootingBehaviour();
        }
    }

    protected override void FixedBehaviour()
    {
        base.FixedBehaviour();
    }
}
