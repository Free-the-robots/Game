using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHeart : MonoBehaviour
{
    private Spaceship spaceship;
    // Start is called before the first frame update
    void Start()
    {
        spaceship = GetComponentInParent<Spaceship>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth(int damage)
    {
        spaceship.loseHealth(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
