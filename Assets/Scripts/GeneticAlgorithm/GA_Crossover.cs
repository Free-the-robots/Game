using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GA.Crossover
{
    public class NEAT_Crossover : GA_Crossover<NEAT.Person>
    {
        public override NEAT.Person crossover(NEAT.Person person1, NEAT.Person person2)
        {
            NEAT.Person res = ScriptableObject.CreateInstance<NEAT.Person>();

            var dissimilar1 = person1.node_connect.Where(n => !person2.node_connect.Select(n1 => n1.innov).Contains(n.innov));
            var dissimilar2 = person2.node_connect.Where(n => !person1.node_connect.Select(n1 => n1.innov).Contains(n.innov));
            var similar = person1.node_connect.Where(n => person2.node_connect.Select(n1 => n1.innov).Contains(n.innov));
            var innov = similar.Select(n => n.innov);

            res.node_gene = new List<NEAT.GENES.Nodes>(person1.node_gene.Union(person2.node_gene).ToList());

            List<int> id = innov.Union(dissimilar1.Union(dissimilar2).Select(n => n.innov)).OrderBy(n=>n).ToList();

            for(int i = 0; i < id.Count; ++i)
            {
                NEAT.GENES.Connection conn1 = person1.node_connect.Find(n => n.innov == id[i]);
                NEAT.GENES.Connection conn2 = person2.node_connect.Find(n => n.innov == id[i]);

                var nbNodes = res.node_gene.Select(n => n.nb);
                if (conn1 == null)
                {
                    res.node_connect.Add(conn2);

                    NEAT.GENES.Nodes inN = person2.node_gene[conn2.inNode];
                    NEAT.GENES.Nodes outN = person2.node_gene[conn2.outNode];
                    if (!nbNodes.Contains(inN.nb))
                        res.node_gene.Add(inN);
                    if (!nbNodes.Contains(outN.nb))
                        res.node_gene.Add(outN);
                }
                else if(conn2 == null)
                {
                    res.node_connect.Add(conn1);

                    NEAT.GENES.Nodes inN = person1.node_gene[conn1.inNode];
                    NEAT.GENES.Nodes outN = person1.node_gene[conn1.outNode];
                    if (!nbNodes.Contains(inN.nb))
                        res.node_gene.Add(inN);
                    if (!nbNodes.Contains(outN.nb))
                        res.node_gene.Add(outN);
                }
                else
                {
                    if(conn1.innov < conn2.innov)
                        res.node_connect.Add(conn1.Clone());
                    else
                        res.node_connect.Add(conn2.Clone());
                    res.node_connect.Last().w = (conn1.w + conn2.w) / 2.0f;

                    if (!conn1.enabled || !conn2.enabled)
                        res.node_connect.Last().enabled = false;

                    NEAT.GENES.Nodes inN = person1.node_gene[conn1.inNode];
                    NEAT.GENES.Nodes outN = person1.node_gene[conn1.outNode];
                    if (!nbNodes.Contains(inN.nb))
                        res.node_gene.Add(inN);
                    if (!nbNodes.Contains(outN.nb))
                        res.node_gene.Add(outN);
                }

            }

            res.node_gene.OrderBy(n => n.nb);

            return res;
        }
    }
}
