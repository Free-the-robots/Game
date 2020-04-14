using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedGuidedEnnemySpaceship : EnnemyAISpaceship
{
    public float lookRadius = 10f;
    protected Transform target;

    private float t = 0f;

    protected override void Setup()
    {
        base.Setup();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Behaviour()
    {
        base.Behaviour();

        if (Vector3.Distance(target.position, transform.position) < lookRadius)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    protected override void FixedBehaviour()
    {
        base.FixedBehaviour();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
