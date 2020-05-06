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
        //protected MaterialPropertyBlock materialBlock;

        protected float t = 0f;
        protected float texTime = 0f;
        //protected int texId = 0;
        public bool destroyed = true;

//       private bool animated = false;

        protected virtual void OnEnable()
        {
            destroyed = false;
//            if (data.skin.Count > 0)
//                animated = true;
            body = GetComponent<Rigidbody>();
            rendererComp = transform.GetChild(0).GetComponent<Renderer>();

            rendererComp.material.mainTexture = data.skin;

            //materialBlock = new MaterialPropertyBlock();
            //// Get the current value of the material properties in the renderer.
            //rendererComp.GetPropertyBlock(materialBlock, 0);
            //// Assign our new value.
            //float[] m_arrTextureArr = new float[1];
            //m_arrTextureArr[0] = 0;
            //materialBlock.SetFloatArray("_TextureIdx", m_arrTextureArr);
            //// Apply the edited values to the renderer.
            //rendererComp.SetPropertyBlock(materialBlock, 0);

            t = 0f;
            texTime = 0f;
        }

        protected virtual void Update()
        {
            t += Time.deltaTime;
            texTime += Time.deltaTime;

            //if(texTime > 1f/data.frequencyImage)
            //{
            //    texTime = 0f;
            //    texId = (texId + 1) % data.skin.Count;

            //    //// Get the current value of the material properties in the renderer.
            //    //rendererComp.GetPropertyBlock(materialBlock, 0);
            //    //// Assign our new value.
            //    //float[] m_arrTextureArr = new float[1];
            //    //m_arrTextureArr[0] = texId;
            //    //materialBlock.SetFloat("_TextureIdx", texId);
            //    //// Apply the edited values to the renderer.
            //    //rendererComp.SetPropertyBlock(materialBlock, 0);

            //    //rendererComp.material.mainTexture = data.skin[texId];
            //}

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
