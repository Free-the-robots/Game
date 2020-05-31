using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Projectiles;

public class ParticlePooling : MonoBehaviour
{
    public GameObject particle;
    //public GameObject particleEvolutive;
    //public GameObject particleHoming;
    //public GameObject particleLaser;
    //public GameObject particleLaserEvo;

    private List<GameObject> pool;
    //private List<GameObject> poolEvolutive;
    //private List<GameObject> poolHoming;
    //private List<GameObject> poolLaser;
    //private List<GameObject> poolLaserEvo;

    public Transform poolParticleTransform;
    //public Transform poolParticleEvolutiveTransform;
    //public Transform poolParticleHomingTransform;
    //public Transform poolParticleLaserTransform;
    //public Transform poolParticleLaserEvoTransform;

    public Transform activesTransform;
    //public Transform activesEvolutiveTransform;
    //public Transform activesHomingTransform;
    //public Transform activesLaserTransform;
    //public Transform activesLaserEvoTransform;

    delegate T ActionRef<T>(string tag, Transform transform, ProjectileData part, int layer);
    private Dictionary<Type, ActionRef<GameObject>> typeInstantiateMap = new Dictionary<Type, ActionRef<GameObject>>();
    private Dictionary<Type, Action<GameObject>> typeKillMap = new Dictionary<Type, Action<GameObject>>();

    public int nb = 1000;

    private static ParticlePooling instance;

    public static ParticlePooling Instance { get { return instance; } }

    private List<Texture2D> listParticles = new List<Texture2D>();
    private Texture2DArray textureArray = null;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this);

        typeInstantiateMap[typeof(ProjectileData)] = particleInstantiate;
        typeInstantiateMap[typeof(ProjectileMultipleStandard)] = particleInstantiate;
        typeInstantiateMap[typeof(ProjectileEvolutiveData)] = particleInstantiateEvolutive;
        typeInstantiateMap[typeof(ProjectileGuidedData)] = particleInstantiateHoming;
        typeInstantiateMap[typeof(ProjectileConeData)] = particleInstantiateCone;
        typeInstantiateMap[typeof(LaserData)] = particleInstantiateLaser;
        typeInstantiateMap[typeof(LaserEvolutiveData)] = particleInstantiateLaserEvo;

        typeKillMap[typeof(ProjectileData)] = destroyCommon;
        typeKillMap[typeof(ProjectileMultipleStandard)] = destroyCommon;
        typeKillMap[typeof(ProjectileEvolutiveData)] = destroyCommon;
        typeKillMap[typeof(ProjectileGuidedData)] = destroyCommon;
        typeKillMap[typeof(ProjectileConeData)] = destroyCommon;
        typeKillMap[typeof(LaserData)] = destroyCommon;
        typeKillMap[typeof(LaserEvolutiveData)] = destroyCommon;
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
        //poolEvolutive = new List<GameObject>(nb);
        //for (int i = 0; i < nb; i++)
        //{
        //    GameObject par = GameObject.Instantiate(particleEvolutive, poolParticleEvolutiveTransform);
        //    par.SetActive(false);
        //    poolEvolutive.Add(par);
        //}
        //poolHoming = new List<GameObject>(nb);
        //for (int i = 0; i < nb; i++)
        //{
        //    GameObject par = GameObject.Instantiate(particleHoming, poolParticleHomingTransform);
        //    par.SetActive(false);
        //    poolHoming.Add(par);
        //}
        //poolLaser = new List<GameObject>(nb);
        //for (int i = 0; i < nb; i++)
        //{
        //    GameObject par = GameObject.Instantiate(particleLaser, poolParticleLaserTransform);
        //    par.SetActive(false);
        //    poolLaser.Add(par);
        //}
        //poolLaserEvo = new List<GameObject>(nb);
        //for (int i = 0; i < nb; i++)
        //{
        //    GameObject par = GameObject.Instantiate(particleLaserEvo, poolParticleLaserEvoTransform);
        //    par.SetActive(false);
        //    poolLaserEvo.Add(par);
        //}
    }

    public GameObject instantiate(string tag, Transform transform, ProjectileData part, int layer)
    {
        return typeInstantiateMap[part.GetType()](tag, transform, part, layer);
    }

    public void destroy(GameObject particle)
    {
        ParticleChooser component = particle.GetComponent<ParticleChooser>();
        typeKillMap[component.active.data.GetType()](particle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //**************************MAPPING**********************/////
    private GameObject particleInstantiate(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);

            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().particle, part);
        }
        return res;
    }

    private GameObject particleInstantiateEvolutive(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().particleEvo, part);

            if (((ProjectileEvolutiveData)part).behaviour.network == null)
                ((ProjectileEvolutiveData)part).behaviour.buildModel();
        }
        return res;
    }

    private GameObject particleInstantiateHoming(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().particleHoming, part);
        }
        return res;
    }

    private GameObject particleInstantiateCone(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().particleCone, part);
        }
        return res;
    }

    private GameObject particleInstantiateLaser(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().laser, part);
        }
        return res;
    }

    private GameObject particleInstantiateLaserEvo(string tag, Transform transform, ProjectileData part, int layer)
    {
        GameObject res = null;
        if (pool.Count > 0)
        {
            res = particleInstantiateCommon(transform, tag, layer);

            //part.createTextureArray(res.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
            chooseParticle(res.GetComponent<ParticleChooser>(), res.GetComponent<ParticleChooser>().laserevo, part);

            if (((LaserEvolutiveData)part).behaviour.network == null)
                ((LaserEvolutiveData)part).behaviour.buildModel();
        }
        return res;
    }

    private void chooseParticle(ParticleChooser chooser, Particle classObj, ProjectileData part)
    {
        classObj.data = part;
        chooser.active = classObj;

        if (!listParticles.Contains(part.skin[0]))
        {
            chooser.active.texIdStart = listParticles.Count;
            listParticles.AddRange(part.skin);
            textureArray = new Texture2DArray(part.skin[0].width, part.skin[0].height, listParticles.Count, part.skin[0].format, false);

            for (int i = 0; i < listParticles.Count; i++)
                Graphics.CopyTexture(listParticles[i], 0, 0, textureArray, i, 0);

            chooser.GetComponent<Renderer>().transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.SetTexture("_Textures", textureArray);
            //part.textureArray = textureArray;
        }
        else
        {
            chooser.active.texIdStart = listParticles.IndexOf(part.skin[0]);
        }
        chooser.active.enabled = true;
    }

    private GameObject particleInstantiateCommon(Transform shipTransform, string tagShip, int layer)
    {
        GameObject res = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        res.transform.parent = activesTransform;
        res.transform.position = shipTransform.position;
        res.transform.rotation = shipTransform.rotation;

        res.GetComponent<Particle>().shooterTag = tagShip;

        res.GetComponent<TrailRenderer>().Clear();

        res.layer = layer;
        res.SetActive(true);

        return res;
    }

    private void destroyCommon(GameObject particle)
    {
        particle.GetComponent<ParticleChooser>().active.destroyed = true;
        particle.GetComponent<ParticleChooser>().active.enabled = false;
        particle.GetComponent<ParticleChooser>().active.data = null;
        particle.GetComponent<ParticleChooser>().active = null;

        particle.SetActive(false);
        particle.transform.parent = poolParticleTransform;
        pool.Add(particle);
    }
}
