using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NN
{
    public class Net
    {
        public List<Node> inNodes = new List<Node>();
        public List<Node> outNodes = new List<Node>();
        public List<Node> hidNodes = new List<Node>();

        public Net(List<NEAT.GENES.Nodes> nodes, List<NEAT.GENES.Connection> connections)
        {
            Dictionary<int,Node> nodes_NN = new Dictionary<int,Node>();
            foreach(NEAT.GENES.Nodes node in nodes)
            {
                Node a = NodeFactory.Create(node.activation);
                a.nb = node.nb;
                nodes_NN[node.nb] = a;
            }

            foreach (NEAT.GENES.Connection connection in connections)
            {
                if (connection.enabled)
                {
                    if (nodes_NN[connection.outNode].inNodes == null)
                        nodes_NN[connection.outNode].inNodes = new List<Node>();
                    nodes_NN[connection.outNode].inNodes.Add(nodes_NN[connection.inNode]);

                    if (nodes_NN[connection.inNode].outNodes == null)
                        nodes_NN[connection.inNode].outNodes = new List<Node>();
                    nodes_NN[connection.inNode].outNodes.Add(nodes_NN[connection.outNode]);

                    nodes_NN[connection.inNode].outW.Add(connection.w);
                    nodes_NN[connection.outNode].w.Add(connection.w);
                }
            }

            foreach(NEAT.GENES.Nodes node in nodes)
            {
                if(node.property == NEAT.GENES.NODE.IN)
                {
                    inNodes.Add(nodes_NN[node.nb]);
                }
                if (node.property == NEAT.GENES.NODE.OUT)
                {
                    outNodes.Add(nodes_NN[node.nb]);
                }
                if (node.property == NEAT.GENES.NODE.HIDDEN)
                {
                    hidNodes.Add(nodes_NN[node.nb]);
                }

            }
        }

        public List<float> evaluate(List<float> inputs)
        {
            List<float> res = new List<float>();

            for(int i = 0; i < outNodes.Count; ++i)
            {
                res.Add(outNodes[i].evaluateBack(inputs));
            }

            return res;
        }

        public void randomizeWeight()
        {
            foreach (Node inN in inNodes)
            {
                for (int i = 0; i < inN.w.Count; ++i)
                {
                    inN.w[i] = Random.Range(-0.5f, 0.5f);
                }
            }
            foreach (Node outN in outNodes)
            {
                for (int i = 0; i < outN.w.Count; ++i)
                {
                    outN.w[i] = Random.Range(-0.5f, 0.5f);
                }
            }
            foreach (Node hidN in hidNodes)
            {
                for (int i = 0; i < hidN.w.Count; ++i)
                {
                    hidN.w[i] = Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }
}
