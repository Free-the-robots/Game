using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spaceship : MonoBehaviour
{
    public List<Weapon.Turret> weapon = new List<Weapon.Turret>();

    protected Rigidbody rbody = null;

    void Start()
    {
        weapon.AddRange(GetComponentsInChildren<Weapon.Turret>().ToList());
        rbody = GetComponent<Rigidbody>();

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
}
