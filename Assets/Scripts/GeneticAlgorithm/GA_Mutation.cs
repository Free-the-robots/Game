using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace GA.Mutation
{
    public class NEAT_Mutation : GA_Mutation<NEAT.Person>
    {
        public override NEAT.Person mutation(NEAT.Person person)
        {
            NEAT.Person res = person.Clone();
            float r = UnityEngine.Random.value;
            if(r > 0.2f)
            {
                List<Tuple<int, int>> innout = new List<Tuple<int, int>>();
                for(int i = 0; i < res.node_gene.Count; ++i)
                {
                    if (res.node_gene[i].property != NEAT.GENES.NODE.OUT)
                    {
                        for (int j = i + 1; j < res.node_gene.Count; ++j)
                        {
                            if (res.node_gene[j].property != NEAT.GENES.NODE.IN)
                                innout.Add(new Tuple<int, int>(res.node_gene[i].nb, res.node_gene[j].nb));
                        }
                    }
                }
                List<Tuple<int, int>> connectionsPossible = innout.Where(
                    p=>!res.node_connect.Select(n=>n.inNode).Contains(p.Item1)
                || !res.node_connect.Select(n => n.outNode).Contains(p.Item2)).ToList();

                if (connectionsPossible.Count > 0)
                {
                    System.Random random = new System.Random();
                    int rr = random.Next(connectionsPossible.Count);
                    int nb1 = connectionsPossible[rr].Item1;
                    int nb2 = connectionsPossible[rr].Item2;

                    NEAT.GENES.Connection connection;
                    int indexC = NEAT.GENES.Connection.alreadyExists(nb1,nb2);
                    if (indexC > -1)
                    {
                        connection = NEAT.GENES.Connection.existing_connections[indexC].Clone();
                        connection.w = UnityEngine.Random.Range(-1f, 1f);
                        connection.enabled = true;
                    }
                    else
                    {
                        connection = ScriptableObject.CreateInstance<NEAT.GENES.Connection>();
                        connection.init(nb1, nb2, UnityEngine.Random.Range(-1f, 1f), true);
                        NEAT.GENES.Connection.existing_connections.Add(connection);
                    }
                    //NEAT.GENES.Connection connection = new NEAT.GENES.Connection(nb1, nb2, UnityEngine.Random.Range(-1f, 1f), true);

                    res.node_connect.Add(connection);
                }
            }
            else
            {
                System.Random random = new System.Random();
                int ran = random.Next(res.node_connect.Count);
                res.node_connect[ran] = res.node_connect[ran].Clone();
                res.node_connect[ran].enabled = false;

                NEAT.GENES.Nodes newn = ScriptableObject.CreateInstance<NEAT.GENES.Nodes>();
                newn.activation = (NEAT.GENES.ACTIVATION)random.Next(8);
                newn.nb = res.node_gene.Select(n => n.nb).Max()+1;
                newn.property = NEAT.GENES.NODE.HIDDEN;


                NEAT.GENES.Connection con1;
                NEAT.GENES.Connection con2;
                int indexC = NEAT.GENES.Connection.alreadyExists(res.node_connect[ran].inNode, newn.nb);
                if (indexC > -1)
                {
                    con1 = NEAT.GENES.Connection.existing_connections[indexC].Clone();
                    con1.w = 1f;
                    con1.enabled = true;
                }
                else
                {
                    con1 = ScriptableObject.CreateInstance<NEAT.GENES.Connection>();
                    con1.init(res.node_connect[ran].inNode, newn.nb, 1f, true);
                    NEAT.GENES.Connection.existing_connections.Add(con1);
                }

                indexC = NEAT.GENES.Connection.alreadyExists(newn.nb, res.node_connect[ran].outNode);
                if (indexC > -1)
                {
                    con2 = NEAT.GENES.Connection.existing_connections[indexC].Clone();
                    con1.w = res.node_connect[ran].w;
                    con1.enabled = true;
                }
                else
                {
                    con2 = ScriptableObject.CreateInstance<NEAT.GENES.Connection>();
                    con2.init(newn.nb, res.node_connect[ran].outNode, res.node_connect[ran].w, true);
                    NEAT.GENES.Connection.existing_connections.Add(con2);
                }
                
                //NEAT.GENES.Connection con1 = new NEAT.GENES.Connection(res.node_connect[ran].inNode, newn.nb, 1f, true);
                //NEAT.GENES.Connection con2 = new NEAT.GENES.Connection(newn.nb, res.node_connect[ran].outNode, res.node_connect[ran].w, true);

                res.node_gene.Add(newn);
                res.node_gene = res.node_gene.OrderBy(n => n.nb).ToList();

                res.node_connect.Add(con1);
                res.node_connect.Add(con2);

            }
            return res;
        }
    }
}
