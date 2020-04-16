using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spaceship : MonoBehaviour
{
    public SpaceshipData spaceshipData;
    public List<Weapon.Turret> weapon = new List<Weapon.Turret>();

    protected Rigidbody rbody = null;

    protected bool alive = true;

    void Start()
    {
        weapon.AddRange(GetComponentsInChildren<Weapon.Turret>().ToList());
        rbody = GetComponent<Rigidbody>();
        alive = true;

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

    public virtual void loseHealth(int damage)
    {
        if (alive)
        {
            spaceshipData.life -= damage;
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
