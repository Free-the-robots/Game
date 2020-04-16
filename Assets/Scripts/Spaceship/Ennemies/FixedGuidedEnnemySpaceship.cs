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
        t += Time.deltaTime;
        base.Behaviour();

        if (Vector3.Distance(target.position, transform.position) < triggerSize)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            if (t > 1F / spaceshipData.freq)
            {
                for (int i = 0; i < weapon.Count; ++i)
                {
                    weapon[i].Fire();
                }
                t = 0f;
            }
        }
    }

    protected override void FixedBehaviour()
    {
        base.FixedBehaviour();
    }
}
