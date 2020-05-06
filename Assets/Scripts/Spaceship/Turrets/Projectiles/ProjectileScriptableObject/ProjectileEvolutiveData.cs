using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileEvolutive", menuName = "Projectiles/Evolutive", order = 3)]
    public class ProjectileEvolutiveData : ProjectileData
    {
        public NEAT.Person behaviour;
    }
}
