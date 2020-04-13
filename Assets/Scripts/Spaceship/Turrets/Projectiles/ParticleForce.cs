using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleForce : Particle
{
    protected Vector3 forceFront = new Vector3(0f, 0f, 1f);
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
        Vector3 vel = (new Vector3(res[1], 0f, res[0]) + forceFront) * 4f;

        apply(vel);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}
