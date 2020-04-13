using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NEAT
{
    namespace GENES
    {
        [System.Serializable, CreateAssetMenu(fileName = "Connection", menuName = "NEAT/Connection", order = 1)]
        public class Connection : ScriptableObject
        {
            public static int global_innov = 0;
            public static List<Connection> existing_connections = new List<Connection>();

            [SerializeField]
            public int inNode = 0;
            [SerializeField]
            public int outNode = 0;

            [SerializeField]
            public float w = 0.0f;

            [SerializeField]
            public bool enabled = true;

            public int innov = 0;

            public static void addConnection(Connection conn)
            {
                if (!existing_connections.Select(n => n.innov).Contains(conn.innov))
                    existing_connections.Add(conn);
            }

            public static int alreadyExists(Connection conn)
            {
                int res = -1;
                var sameConnection = existing_connections.Where(n => n.inNode == conn.inNode && n.outNode == conn.outNode).First();
                if (existing_connections.Contains(sameConnection))
                    res = sameConnection.innov;
                return res;
            }

            public static int alreadyExists(int inNode, int outNode)
            {
                int res = -1;
                var sameConnection = existing_connections.Where(n => n.inNode == inNode && n.outNode == outNode).FirstOrDefault();
                if (sameConnection != null && existing_connections.Contains(sameConnection))
                    res = existing_connections.IndexOf(sameConnection);
                return res;
            }

            public void instantiate()
            {
                innov = global_innov++;
            }

            public Connection()
            {

            }

            public void init(int inN, int outN, float w, bool enable)
            {
                innov = global_innov++;
                inNode = inN;
                outNode = outN;
                this.w = w;
                enabled = enable;
            }

            public Connection(int inN, int outN, float w, bool enable)
            {
                init(inN, outN, w, enable);
            }

            public Connection Clone()
            {
                //Connection res = new Connection();
                Connection res = ScriptableObject.CreateInstance<NEAT.GENES.Connection>();
                res.inNode = inNode;
                res.outNode = outNode;
                res.w = w;
                res.innov = innov;
                res.enabled = enabled;
                return res;
            }

            public override string ToString()
            {
                string res = "Conn : " + innov + "\n";
                res += "In : " + inNode + ", Out : " + outNode + "\n";
                res += "w : " + w + ", enabled : " + enabled + "\n\n";
                return res;
            }
        }
    }
}
