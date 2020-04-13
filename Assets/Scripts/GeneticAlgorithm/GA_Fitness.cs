using System.Collections;
using System.Collections.Generic;
using NEAT;
using UnityEngine;

namespace GA.Fitness
{
    public class NEAT_Fitness : GA_Fitness<NEAT.Person>
    {
        public override float fitness(Person person)
        {
            return person.usage;
        }
    }
}
