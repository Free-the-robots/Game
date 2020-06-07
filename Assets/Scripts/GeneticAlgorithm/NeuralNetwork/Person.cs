using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using SimpleJSON;

namespace NEAT
{
    [System.Serializable, CreateAssetMenu(fileName = "Person", menuName = "NEAT/Person", order = 0)]
    public class Person : ScriptableObject
    {
        public List<GENES.Nodes> node_gene = new List<GENES.Nodes>();
        public List<GENES.Connection> node_connect = new List<GENES.Connection>();
        public int usage = 0;

        public NN.Net network = null;

        public List<float> evaluate(List<float> inputs)
        {
            if (network == null)
                buildModel();
            int inNodesN = node_gene.Where(node => node.property == GENES.NODE.IN).Count();

            if (inputs.Count != inNodesN)
                return null;

            return network.evaluate(inputs);
        }

        public void buildModel()
        {
            network = new NN.Net(node_gene, node_connect);
        }

        public void instantiate()
        {
            foreach (GENES.Connection connection in node_connect)
            {
                connection.instantiate();
            }
	    }

        public float distance(Person person)
        {
            float N = Mathf.Max(node_gene.Count, person.node_gene.Count);
            if (N < 20)
                N = 1f;

            var innov1 = node_connect.Select(n => n.innov);
            var innov2 = person.node_connect.Select(n => n.innov);
            //Similar Edges
            var innovSimilar = innov1.Intersect(innov2);

            //Disjoint Edges
            var unionDissimilar = innov1.Except(innov2).Union(innov2.Except(innov1));
            int disjoints = unionDissimilar.Count();

            float weaverage = 0f;

            foreach(int innov in innovSimilar)
            {
                weaverage += Mathf.Abs(node_connect.Find(n => n.innov == innov).w - person.node_connect.Find(n => n.innov == innov).w);
            }
            if(innovSimilar.Count() != 0)
                weaverage /= innovSimilar.Count();

            float c1 = 0.5f;
            float c2 = 0.5f;

            return disjoints*c1/N + weaverage * c2;
        }

        public override string ToString()
        {
            string res = "Nodes Count : " + node_gene.Count + ", Node Connections" + node_connect.Count + "\n";
            res += "In : " + node_gene.Where(p => p.property == GENES.NODE.IN).Count() + ", Out :" + node_gene.Where(p => p.property == GENES.NODE.OUT).Count() + "\n\n";
            foreach (GENES.Nodes node in node_gene)
            {
                res += node + ", ";
            }
            res += "\n";
            foreach (GENES.Connection conn in node_connect)
            {
                res += conn;
            }
            return res;
        }

        public Person Clone()
        {
            Person res = ScriptableObject.CreateInstance<NEAT.Person>();
            res.node_connect = new List<GENES.Connection>(node_connect);
            res.node_gene = new List<GENES.Nodes>(node_gene);
            return res;
        }

        public void Save()
        {
            string json = "{\n\t\"node_gene\": [\n";
            foreach (GENES.Nodes node in node_gene)
            {
                json += node.GetInstanceID() + " " + JsonUtility.ToJson(node, true) + ",\n";
            }
            json += "\n\t],\n\t\"node_connect\": [\n";
            foreach (GENES.Connection connection in node_connect)
            {
                json += connection.GetInstanceID() + " " + JsonUtility.ToJson(connection, true) + ",\n";
            }
            json += "\n\t]\n}";
            File.WriteAllText(Application.dataPath + Path.DirectorySeparatorChar + "AvatarData.txt", json);
            Debug.Log("Saved to : " + Application.dataPath + Path.DirectorySeparatorChar + "AvatarData.txt");
        }

        public void fromJson(string data)
        {
            JSONNode N = JSON.Parse(data);
            node_gene = new List<GENES.Nodes>(N["node_gene"].Count);
            for(int i = 0; i < N["node_gene"].Count; ++i)
            {
                JSONNode Nsub = N["node_gene"][i];
                GENES.Nodes obj = ScriptableObject.CreateInstance<GENES.Nodes>();
                //JsonUtility.FromJsonOverwrite(N["node_gene"][i], obj);
                obj.activation = (GENES.ACTIVATION)Nsub["activation"].AsInt;
                obj.property = (GENES.NODE)Nsub["property"].AsInt;
                obj.nb = Nsub["nb"].AsInt;
                node_gene.Add(obj);
                //Debug.Log(N["node_gene"][i]);
            }
            node_connect = new List<GENES.Connection>(N["node_connect"].Count);
            for (int i = 0; i < N["node_connect"].Count; ++i)
            {
                JSONNode Nsub = N["node_connect"][i];
                GENES.Connection obj = ScriptableObject.CreateInstance<GENES.Connection>();
                //JsonUtility.FromJsonOverwrite(N["node_connect"][i], obj);
                obj.inNode = Nsub["inNode"].AsInt;
                obj.outNode = Nsub["outNode"].AsInt;
                obj.w = Nsub["w"].AsFloat;
                obj.enabled = Nsub["enabled"].AsBool;
                obj.innov = Nsub["innov"].AsInt;
                node_connect.Add(obj);
                //Debug.Log(N["node_connect"][i]);
            }
        }
    }
}
