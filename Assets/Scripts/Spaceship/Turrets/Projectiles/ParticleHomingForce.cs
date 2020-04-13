using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHomingForce : ParticleForce
{
    public Transform target;
    public float forceScale = 50f;
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        evaluate();

        List<float> res = weapon.evaluate(inputs);
        if(target != null)
        {
            forceFront = (target.position - transform.position).normalized;
        }
        Vector3 vel = (new Vector3(res[1], 0f, res[0]) + forceFront) * forceScale;

        apply(vel);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}
