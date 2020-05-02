using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    public class Particle : MonoBehaviour
    {
        public ProjectileData data;

        public string shooterTag = "";
        protected Rigidbody body;

        protected float t = 0f;
        protected virtual void OnEnable()
        {
            body = GetComponent<Rigidbody>();

            t = 0f;
        }

        protected virtual void Update()
        {
            t += Time.deltaTime;
        }

        protected virtual void FixedUpdate()
        {
            apply(new Vector3(0f, 0f, 1f));
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag != shooterTag)
            {
                if (other.transform.tag.Equals("Player"))
                {
                    other.transform.GetComponent<SpaceshipHeart>().loseHealth(10);
                    ParticlePooling.Instance.destroy(this.gameObject);
                }
                else if (other.transform.tag.Equals("Enemy"))
                {
                    other.transform.GetComponent<Spaceship>().loseHealth(10);
                    ParticlePooling.Instance.destroy(this.gameObject);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag != shooterTag)
            {
                //if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Enemy"))
                //{
                //    other.transform.GetComponent<Spaceship>().loseHealth(10);
                //    GameObject.Destroy(this.gameObject);
                //}

                if (other.transform.tag.Equals("Obstacles"))
                {
                    transform.forward = Vector3.Reflect(transform.forward, other.GetContact(0).normal);
                }
            }
        }

        protected virtual void apply(Vector3 vel)
        {
            body.velocity = transform.TransformDirection(vel.normalized * data.velocity);

            if (t > data.lifeTime)
            {
                t = 0f;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
