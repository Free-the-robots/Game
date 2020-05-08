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
        protected Renderer rendererComp;
        protected MaterialPropertyBlock materialBlock;

        protected float t = 0f;
        protected float texTime = 0f;
        protected int texId = 0;
        public int texIdStart = 0;
        public bool destroyed = true;

        private bool animated = false;
        protected virtual void Awake()
        {
            body = GetComponent<Rigidbody>();
            rendererComp = transform.GetChild(0).GetComponent<Renderer>();
            materialBlock = new MaterialPropertyBlock();
        }

        protected virtual void OnEnable()
        {
            destroyed = false;
            if (data.skin.Count > 1)
                animated = true;

            //rendererComp.material.mainTexture = data.skin;

            // Get the current value of the material properties in the renderer.
            rendererComp.GetPropertyBlock(materialBlock, 0);
            // Assign our new value.
            materialBlock.SetFloat("_TextureIdx", texIdStart + texId);
            // Apply the edited values to the renderer.
            rendererComp.SetPropertyBlock(materialBlock, 0);

            t = 0f;
            texTime = 0f;
        }

        protected virtual void Update()
        {
            t += Time.deltaTime;
            if (animated)
            {
                texTime += Time.deltaTime;

                if (texTime > 1f / data.frequencyImage)
                {
                    texTime = 0f;
                    texId = (texId + 1) % data.skin.Count;

                    // Get the current value of the material properties in the renderer.
                    rendererComp.GetPropertyBlock(materialBlock, 0);
                    // Assign our new value.
                    materialBlock.SetFloat("_TextureIdx", texIdStart + texId);
                    // Apply the edited values to the renderer.
                    rendererComp.SetPropertyBlock(materialBlock, 0);

                    //rendererComp.material.mainTexture = data.skin[texId];
                }
            }
        }

        protected virtual void OnDisable()
        {
            texId = 0;
            texIdStart = 0;
            destroyed = true;
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
                    ProjectilesSpecials.enemyTriggerBehaviour[data.special](this.gameObject, other);
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
                    ProjectilesSpecials.obstacleBehaviour[data.special](this.gameObject, other);
                }
            }
        }

        protected virtual void apply(Vector3 vel)
        {
            //body.velocity = transform.TransformDirection(vel.normalized * data.velocity);
            transform.position += transform.TransformDirection(vel.normalized * data.velocity * Time.deltaTime);

            if (t > data.lifeTime)
            {
                Debug.Log(data.lifeTime);
                t = 0f;
//                body.velocity = Vector3.zero;
//                body.angularVelocity = Vector3.zero;
                ParticlePooling.Instance.destroy(this.gameObject);
            }
        }
    }
}
