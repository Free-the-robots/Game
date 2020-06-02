using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    //TODO : plusieurs solutions : les proj sont arrêté par le shield, les proj prennent un malus quand ils passent par le shield
    //les proj qui touchent le shield charge une jauge qui augmente les degat du vaisseau
    public class SquareShield : Abilities
    {
        [SerializeField]
        private GameObject Shield;

        public override void RunAbility(Transform transform)
        {
            Instantiate(Shield, transform.position + (transform.forward*2), transform.rotation);
        }

    }
}

