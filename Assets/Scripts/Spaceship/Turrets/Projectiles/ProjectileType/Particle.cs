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
        public bool destroyed = true;

        protected virtual void OnEnable()
        {
            destroyed = false;
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
            if (destroyed)
            {
                return;
            }

            if (other.transform.tag != shooterTag)
            {
                if (other.transform.tag.Equals("Player"))
                {
                    if (other.transform.GetComponent<SpaceshipHeart>())
                        other.transform.GetComponent<SpaceshipHeart>().loseHealth(data.damage);
                    ParticlePooling.Instance.destroy(this.gameObject);
                }
                else if (other.transform.tag.Equals("Enemy"))
                {
                    if(other.transform.GetComponent<Spaceship>())
                        other.transform.GetComponent<Spaceship>().loseHealth(data.damage);
                    //ProjectilesSpecials.enemyTriggerBehaviour[data.special](this.gameObject, other);

                    ParticlePooling.Instance.destroy(this.gameObject);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (destroyed)
                return;

            if (other.transform.tag != shooterTag)
            {
                //if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Enemy"))
                //{
                //    other.transform.GetComponent<Spaceship>().loseHealth(10);
                //    GameObject.Destroy(this.gameObject);
                //}

                if (other.transform.tag.Equals("Obstacles"))
                {
                    //ProjectilesSpecials.obstacleBehaviour[data.special](this.gameObject, other);
                }
            }
        }

        protected virtual void apply(Vector3 vel)
        {
            body.velocity = transform.TransformDirection(vel.normalized * data.velocity);

            if (t > data.lifeTime)
            {
                t = 0f;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
