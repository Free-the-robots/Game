using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using GA.Selection;
using GA.Crossover;
using GA.Mutation;
using GA.Fitness;

namespace GA
{
    namespace Fitness
    {
        public class GA_Fitness<T>
        {
            public virtual float fitness(T person)
            {
                return 0f;
            }
        }
    }
    namespace Selection
    {
        public class GA_Selection<T>
        {
            public virtual void selection(List<T> population, int size, int eliteSize, List<float> fitnesses, List<Tuple<T, T>> crossoverPop, float crossovers, List<T> mutationPop)
            {

            }
        }
    }
    namespace Crossover
    {
        public class GA_Crossover<T>
        {
            public virtual T crossover(T person1, T person2)
            {
                return default(T);
            }
        }
    }

    namespace Mutation
    {
        public class GA_Mutation<T>
        {
            public virtual T mutation(T person)
            {
                return default(T);
            }
        }
    }

    public class GA<T>
    {
        public List<T> results = new List<T>();
        public float elitesP = 0.05f;
        public float crossoversP = 0.8f;
        public int population_size = 0;
        public List<T> population;
        public List<float> population_fit = null;

        protected GA_Selection<T> selectionObj = null;
        protected GA_Crossover<T> crossoverObj = null;
        protected GA_Mutation<T> mutationObj = null;
        protected GA_Fitness<T> fitnessObj = null;

        protected IEnumerable<T> orderedPop = null;
        protected List<Tuple<T, T>> crossoverPop = null;
        protected List<T> mutationPop = null;

        public GA(List<T> init_population, int size = 10, float elites_count = 0.05f, float crossovers = 0.8f)
        {
            selectionObj = new Roulette<T>();
            crossoverObj = new GA_Crossover<T>();
            mutationObj = new GA_Mutation<T>();
            fitnessObj = new GA_Fitness<T>();

            population = init_population;
            population_size = size;
            elitesP = elites_count;
            crossoversP = crossovers;


            int crossoverPopN = (int)(crossoversP * population_size);
            crossoverPop = new List<Tuple<T, T>>(crossoverPopN - (int)(elitesP * population.Count));
            mutationPop = new List<T>(population_size - crossoverPopN - (int)(elitesP * population.Count));

            population_fit = new List<float>(population.Count);
        }

        public GA(List<T> init_population, GA_Selection<T> selectionType, GA_Crossover<T> crossoverType, GA_Mutation<T> mutationType, GA_Fitness<T> fitnessType, int size = 10, float elites_count = 0.05f, float crossovers = 0.8f)
        {
            selectionObj = selectionType;
            crossoverObj = crossoverType;
            mutationObj = mutationType;
            fitnessObj = fitnessType;

            population = init_population;
            population_size = size;
            elitesP = elites_count;
            crossoversP = crossovers;

            int crossoverPopN = (int)(crossoversP * population_size);
            crossoverPop = new List<Tuple<T, T>>(crossoverPopN - (int)(elitesP*population.Count));
            mutationPop = new List<T>(population_size - crossoverPopN - (int)(elitesP * population.Count));

            population_fit = new List<float>(population.Count);
        }

        public void updatePopulation(List<T> init_population)
        {
            population = init_population;
            int crossoverPopN = (int)(crossoversP * population_size);
            crossoverPop = new List<Tuple<T, T>>(crossoverPopN - (int)(elitesP * population.Count));
            mutationPop = new List<T>(population_size - crossoverPopN - (int)(elitesP * population.Count));

            population_fit = new List<float>(population.Count);
        }

        public void updateParameters(int size = 10, float elites_count = 0.05f, float crossovers = 0.8f)
        {
            population_size = size;
            
            population_size = size;
            elitesP = elites_count;
            crossoversP = crossovers;

            int crossoverPopN = (int)(crossoversP * population_size);
            crossoverPop = new List<Tuple<T, T>>(crossoverPopN - (int)(elitesP * population.Count));
            mutationPop = new List<T>(population_size - crossoverPopN - (int)(elitesP * population.Count));

            population_fit = new List<float>(population.Count);
        }

        public void crossover()
        {
            foreach(Tuple<T,T> parents in crossoverPop)
            {
                results.Add(crossover(parents.Item1, parents.Item2));
            }
        }

        public virtual T crossover(T person1, T person2)
        {
            return crossoverObj.crossover(person1, person2);
        }

        public void mutation()
        {
            foreach (T parent in mutationPop)
                results.Add(mutation(parent));
        }

        public virtual T mutation(T person)
        {
            return mutationObj.mutation(person);
        }

        public virtual void selection()
        {
            orderedPop = population.OrderByDescending(p => fitnessObj.fitness(p));
            population_fit.AddRange(orderedPop.Select(p => fitnessObj.fitness(p)));

            results.AddRange(orderedPop.Take((int)(elitesP*population.Count)));

            selectionObj.selection(orderedPop.ToList(), population_size, (int)(elitesP * population.Count),population_fit, crossoverPop, crossoversP, mutationPop);
        }

        public List<T> breed()
        {
            results.Clear();
            population_fit.Clear();

            orderedPop = null;
            crossoverPop.Clear();
            mutationPop.Clear();

            selection();
            crossover();
            mutation();

            orderedPop = null;
            crossoverPop.Clear();
            mutationPop.Clear();

            return results;
        }
    }
}
