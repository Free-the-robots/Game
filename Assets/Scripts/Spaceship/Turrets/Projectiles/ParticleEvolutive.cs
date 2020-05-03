using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleEvolutive : Particle
    {
        protected List<float> inputs = new List<float>(4) { 0f, 0f, 0f, 1f };

        protected Vector3 initPos;

        // Start is called before the first frame update
        protected override void OnEnable()
        {
            base.OnEnable();

            initPos = transform.position;

            inputs[0] = 0f;
            inputs[1] = 0f;
            inputs[2] = Vector3.Distance(initPos, transform.position);
            inputs[3] = 1f;
        }
        // Update is called once per frame
        protected override void FixedUpdate()
        {
            evaluate();

            List<float> res = ((ProjectileEvolutiveData)data).behaviour.evaluate(inputs);
            Vector3 vel = (new Vector3(res[1], 0f, res[0]) + Vector3.forward).normalized;

            apply(vel);
        }

        protected virtual void evaluate()
        {
            Vector3 pos = transform.InverseTransformDirection(transform.position - initPos);
            inputs[0] = pos.z;
            inputs[1] = pos.x;
            inputs[2] = (Vector3.Distance(initPos, transform.position));
        }
    }
}
