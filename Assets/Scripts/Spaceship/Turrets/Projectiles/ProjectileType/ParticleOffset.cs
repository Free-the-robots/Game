using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class ParticleOffset : ParticleEvolutive
    {
        // Start is called before the first frame update
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        // Update is called once per frame
        protected override void FixedUpdate()
        {
            evaluate();

            List<float> res = ((ProjectileEvolutiveData)data).behaviour.evaluate(inputs);
            Vector3 vel = new Vector3(res[1] * 50f, 0f, ((res[0] + 1f) / 2f) * 50f);

            apply(vel);
        }

        protected override void apply(Vector3 vel)
        {
            vel.x = Mathf.Sin(vel.z * Mathf.PI / 2f) * vel.x;
            body.velocity = transform.TransformDirection(vel);

            if (t > data.lifeTime)
            {
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}