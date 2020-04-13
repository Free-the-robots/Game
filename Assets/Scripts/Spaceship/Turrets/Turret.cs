using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Turret : MonoBehaviour
    {
        public NEAT.Person chosenW = null;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Fire()
        {
            ParticlePooling.Instance.instantiate(this.tag, transform, chosenW);
        }
    }
}
