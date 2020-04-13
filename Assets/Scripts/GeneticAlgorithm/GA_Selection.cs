using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GA.Selection
{
    public class Roulette<T> : GA_Selection<T>
    {
        public override void selection(List<T> population, int size, int eliteSize, List<float> fitnesses, List<Tuple<T, T>> crossoverPop, float crossovers, List<T> mutationPop)
        {
            float sumFit = fitnesses.Sum();
            int crossoverCount = (int)(crossovers * size) - eliteSize;
            for(int i = 0; i < crossoverCount; ++i)
            {
                double sum = 0;
                List<double> cumulatedSum = fitnesses.Select(f => sum += f / sumFit).ToList();
                float r = UnityEngine.Random.value;
                int index = cumulatedSum.FindIndex(f => f > r);
                if (index == -1)
                    index = 0;

                crossoverPop.Add(new Tuple<T,T>(population[index], population[(index+1)%population.Count]));
            }
            int mutationCount = size - crossoverCount - eliteSize;
            for (int i = 0; i < mutationCount; ++i)
            {
                double sum = 0;
                List<double> cumulatedSum = fitnesses.Select(f => sum += f / sumFit).ToList();

                float r = UnityEngine.Random.value;
                int index = cumulatedSum.FindIndex(f => f > r);
                if (index == -1)
                    index = 0;

                mutationPop.Add(population[index]);
            }
        }
    }

    public class NEAT_Selection : GA_Selection<NEAT.Person>
    {
        public int IndexOfClosestSpeciation(int person, List<NEAT.Person> population)
        {
            NEAT.Person best_match = population.Where((p, i) => i != person).OrderBy(p => population[person].distance(p)).ToList()[(int)(UnityEngine.Random.value*(population.Count/3f))];
            
            return population.IndexOf(best_match);
        }

        public override void selection(List<NEAT.Person> population, int size, int eliteSize, List<float> fitnesses, List<Tuple<NEAT.Person, NEAT.Person>> crossoverPop, float crossovers, List<NEAT.Person> mutationPop)
        {
            float sumFit = fitnesses.Sum();
            int crossoverCount = (int)(crossovers * size) - eliteSize;
            for (int i = 0; i < crossoverCount; ++i)
            {
                double sum = 0;
                List<double> cumulatedSum = fitnesses.Select(f => sum += f / sumFit).ToList();

                float r = UnityEngine.Random.value;
                int index = cumulatedSum.FindIndex(f => f > r);
                if (index == -1)
                    index = 0;

                int index2 = IndexOfClosestSpeciation(index, population);
                if (index2 == -1)
                    index2 = 0;

                crossoverPop.Add(new Tuple<NEAT.Person, NEAT.Person>(population[index], population[index2]));
            }
            int mutationCount = size - crossoverCount - eliteSize;
            for (int i = 0; i < mutationCount; ++i)
            {
                double sum = 0;
                List<double> cumulatedSum = fitnesses.Select(f => sum += f / sumFit).ToList();

                float r = UnityEngine.Random.value;
                int index = cumulatedSum.FindIndex(f => f > r);
                if (index == -1)
                    index = 0;

                mutationPop.Add(population[index]);
            }
        }
    }
}
