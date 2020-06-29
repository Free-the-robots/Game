using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spaceship : MonoBehaviour
{
    public SpaceshipData spaceshipData;
    public List<Weapon.Turret> weapon = new List<Weapon.Turret>();

    protected Rigidbody rbody = null;
    protected Renderer rendererComp;

    protected bool alive = true;
    public bool animating = false;

    void Start()
    {
        if (spaceshipData != null)
            spaceshipData = spaceshipData.Clone();
        weapon.AddRange(GetComponentsInChildren<Weapon.Turret>().ToList());
        rbody = GetComponent<Rigidbody>();
        alive = true;
        rendererComp = transform.GetChild(transform.childCount - 1).GetComponent<Renderer>();

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        Behaviour();
    }


    void FixedUpdate()
    {
        FixedBehaviour();
    }

    protected virtual void Setup()
    {

    }

    protected virtual void Behaviour()
    {

    }

    protected virtual void FixedBehaviour()
    {

    }

    protected virtual void setColor(Color color)
    {
        Material mat = rendererComp.sharedMaterial;
        //mat.SetColor("_EmissionColor", color);
        mat.color = color;
    }

    protected virtual IEnumerator loseHealthAnimation()
    {
        float animationgT = 0f;
        float colorR = 0f;
        bool colorFlip = false;
        animating = true;
        while (animating)
        {
            animationgT += Time.deltaTime;
            colorR = Mathf.SmoothStep(1f, 0f, animationgT * 5f);
            if(colorFlip)
                colorR = Mathf.SmoothStep(0f, 1f, animationgT * 5f);
            Color color = new Color(1f, colorR, colorR);
            setColor(color);

            if (animationgT > 0.2f)
            {
                if (colorFlip)
                {
                    animating = false;
                }
                else
                {
                    animationgT = 0f;
                    colorFlip = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public virtual void loseHealth(int damage)
    {
        if (alive)
        {
            spaceshipData.life -= damage;
            StartCoroutine(loseHealthAnimation());
            if (spaceshipData.life <= 0)
            {
                Death();
            }
        }
    }

    public virtual void Death()
    {
        alive = false;
        GameObject.Destroy(this.gameObject);
    }
}
