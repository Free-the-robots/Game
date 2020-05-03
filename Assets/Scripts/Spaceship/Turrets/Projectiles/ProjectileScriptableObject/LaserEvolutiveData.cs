using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "LaserEvolutive", menuName = "Projectiles/LaserEvolutive", order = 4)]
    public class LaserEvolutiveData : LaserData
    {
        public NEAT.Person behaviour;
    }
}
