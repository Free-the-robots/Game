using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Projectiles;

public class ParticlePooling : MonoBehaviour
{
    public GameObject particle;
    public GameObject particleEvolutive;
    public GameObject particleHoming;

    private List<GameObject> pool;
    private List<GameObject> poolEvolutive;
    private List<GameObject> poolHoming;

    public Transform poolParticleTransform;
    public Transform poolParticleEvolutiveTransform;
    public Transform poolParticleHomingTransform;

    public Transform activesTransform;
    public Transform activesEvolutiveTransform;
    public Transform activesHomingTransform;

    delegate T ActionRef<T>(string tag, Transform transform, ProjectileData part, int layer);
    private Dictionary<Type, ActionRef<GameObject>> typeInstantiateMap = new Dictionary<Type, ActionRef<GameObject>>();
    private Dictionary<Type, Action<GameObject>> typeKillMap = new Dictionary<Type, Action<GameObject>>();

    public int nb = 1000;

    private static ParticlePooling instance;

    public static ParticlePooling Instance { get { return instance; } }


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this);


        typeInstantiateMap[typeof(ProjectileData)] = particleInstantiate;
        typeInstantiateMap[typeof(ProjectileEvolutiveData)] = particleInstantiateEvolutive;
        typeInstantiateMap[typeof(ProjectileGuidedData)] = particleInstantiateHoming;

        typeKillMap[typeof(ProjectileData)] = destroyParticle;
        typeKillMap[typeof(ProjectileEvolutiveData)] = destroyEvolutive;
        typeKillMap[typeof(ProjectileGuidedData)] = destroyHoming;
    }

    // Start is called before the first frame update
    void Start()
    {
        pool = new List<GameObject>(nb);
        for (int i = 0; i < nb; i++)
        {
            GameObject par = GameObject.Instantiate(particle, poolParticleTransform);
            par.SetActive(false);
            pool.Add(par);
        }
        poolEvolutive = new List<GameObject>(nb);
        for (int i = 0; i < nb; i++)
        {
            GameObject par = GameObject.Instantiate(particleEvolutive, poolParticleEvolutiveTransform);
            par.SetActive(false);
            poolEvolutive.Add(par);
        }
        poolHoming = new List<GameObject>(nb);
        for (int i = 0; i < nb; i++)
        {
            GameObject par = GameObject.Instantiate(particleHoming, poolParticleHomingTransform);
            par.SetActive(false);
            poolHoming.Add(par);
        }
    }

    public GameObject instantiate(string tag, Transform transform, ProjectileData part, int layer)
    {
        return typeInstantiateMap[part.GetType()](tag, transform, part, layer);
    }

    public void destroy(GameObject particle)
    {
        Particle component = particle.GetComponent<Particle>();
        typeKillMap[component.data.GetType()](particle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //**************************MAPPING**********************/////
    public GameObject particleInstantiate(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(pool, activesTransform, transform, tag, layer);
            res.GetComponent<Particle>().data = part;
        }
        return res;
    }

    public GameObject particleInstantiateEvolutive(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (poolEvolutive.Count > 0)
        {
            res = particleInstantiateCommon(poolEvolutive, activesTransform, transform, tag, layer);
            res.GetComponent<Particle>().data = part;

            if (((ProjectileEvolutiveData)part).behaviour.network == null)
                ((ProjectileEvolutiveData)part).behaviour.buildModel();
        }
        return res;
    }

    public GameObject particleInstantiateHoming(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (poolEvolutive.Count > 0)
        {
            res = particleInstantiateCommon(poolEvolutive, activesTransform, transform, tag, layer);
            res.GetComponent<Particle>().data = part;
        }
        return res;
    }

    public GameObject particleInstantiateCommon(List<GameObject> particlePool, Transform activeTrans, Transform shipTransform, string tagShip, int layer)
    {
        GameObject res = particlePool[particlePool.Count - 1];
        particlePool.RemoveAt(particlePool.Count - 1);
        res.transform.parent = activeTrans;
        res.transform.position = shipTransform.position;
        res.transform.rotation = shipTransform.rotation;

        res.GetComponent<Particle>().shooterTag = tagShip;

        res.GetComponent<Particle>().enabled = true;

        res.layer = layer;
        res.SetActive(true);

        return res;
    }

    public void destroyParticle(GameObject particle)
    {
        particle.GetComponent<Particle>().enabled = false;
        //particle.GetComponent<Particle>().weapon = null;
        particle.SetActive(false);
        particle.transform.parent = poolParticleTransform;
        pool.Add(particle);
    }


    public void destroyEvolutive(GameObject particle)
    {
        particle.GetComponent<Particle>().enabled = false;
        //particle.GetComponent<Particle>().weapon = null;
        particle.SetActive(false);
        particle.transform.parent = poolParticleEvolutiveTransform;
        poolEvolutive.Add(particle);
    }


    public void destroyHoming(GameObject particle)
    {
        particle.GetComponent<Particle>().enabled = false;
        //particle.GetComponent<Particle>().weapon = null;
        particle.SetActive(false);
        particle.transform.parent = poolParticleHomingTransform;
        poolHoming.Add(particle);
    }
}
