using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEAT
{
    namespace GENES
    {
        public enum NODE
        {
            IN,
            OUT,
            HIDDEN
        }

        public enum ACTIVATION
        {
            RAMP,
            SIN,
            COS,
            TANH,
            TAN,
            RECT,
            BIPOLAR,
            GAUSSIAN
        }

        [System.Serializable, CreateAssetMenu(fileName = "Node", menuName = "NEAT/Node", order = 1)]
        public class Nodes : ScriptableObject
        {

            [SerializeField]
            public NODE property = NODE.IN;


            [SerializeField]
            public ACTIVATION activation = ACTIVATION.SIN;

            [SerializeField]
            public int nb = 0;

            public void init(NODE mode, ACTIVATION act, int n)
            {
                property = mode;
                activation = act;
                nb = n;
            }

            public Nodes()
            {
                property = NODE.IN;
                activation = ACTIVATION.SIN;
                nb = 0;
            }

            public Nodes(NODE mode, ACTIVATION act, int n)
            {
                init(mode, act, n);
            }
            
            public override string ToString()
            {
                return "Node (" + nb + "," + property + "," + activation + ")";
            }

            public Nodes Clone()
            {
                Nodes res = ScriptableObject.CreateInstance<NEAT.GENES.Nodes>();
                res.property = property;
                res.activation = activation;
                res.nb = nb;
                return res;
            }
        }
    }
}
