using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedGuidedEnnemySpaceship : EnnemyAISpaceship
{

    protected override void Setup()
    {
        base.Setup();
    }

    float t = 0f;
    protected override void Behaviour()
    {
        base.Behaviour();

        if (Vector3.Distance(target.position, transform.position) < triggerSize)
        {

            t += Time.deltaTime;
            ShootingBehaviour(ref t);
        }
    }

    protected override void FixedBehaviour()
    {
        base.FixedBehaviour();
    }
}
