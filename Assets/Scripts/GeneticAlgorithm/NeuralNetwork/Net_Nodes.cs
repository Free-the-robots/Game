using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN
{
    public class Node
    {
        public List<Node> inNodes = null;
        public List<Node> outNodes = null;
        public List<float> w = new List<float>();
        public List<float> outW = new List<float>();
        public int nb = 0;

        public virtual float activation(float x)
        {
            if (x > 1f)
                x = 1f;
            if (x < -1f)
                x = -1f;
            return x;
        }

        public float evaluateBack(List<float> inputs)
        {
            float sum = 0f;
            if (inNodes == null)
            {
                if (inputs.Count > nb)
                    return activation(inputs[nb]);
                else
                    return 0f;
            }

            for (int i = 0; i < inNodes.Count; ++i)
            {
                sum += w[i] * inNodes[i].evaluateBack(inputs);
            }
            return activation(sum);
        }
    }
    public class Rectifier : Node
    {
        public override float activation(float x)
        {
            if (x >= 0f)
                x = 1f;
            if (x < 0f)
                x = -1f;
            return x;
        }
    }
    public class SinNode : Node
    {
        public override float activation(float x)
        {
            return Mathf.Sin(x);
        }
    }
    public class CosNode : Node
    {
        public override float activation(float x)
        {
            return Mathf.Cos(x);
        }
    }
    public class TanhNode : Node
    {
        public override float activation(float x)
        {
            return (float)System.Math.Tanh(x);
        }
    }
    public class GaussianNode : Node
    {
        public override float activation(float x)
        {
            return 1f / Mathf.Sqrt(0.5f * Mathf.PI) * Mathf.Exp(-(x * x));
        }
    }
    public class BipolarNode : Node
    {
        public override float activation(float x)
        {
            return (1f - Mathf.Exp(-x)) / (1f + Mathf.Exp(-x));
        }
    }
    public class TanNode : Node
    {
        public override float activation(float x)
        {
            return Mathf.Tan(x);
        }
    }

    public abstract class INodeCreator
    {
        public abstract Node Create();
    }
    public class NodeCreator<T> : INodeCreator where T : Node, new()
    {
        public override Node Create()
        {
            return new T();
        }
    }

    public static class NodeFactory
    {
        private static List<INodeCreator> nodes = new List<INodeCreator>() { new NodeCreator<Node>(),
        new NodeCreator<SinNode>(),
        new NodeCreator<CosNode>(),
        new NodeCreator<TanhNode>(),
        new NodeCreator<TanNode>(),
        new NodeCreator<Rectifier>(),
        new NodeCreator<BipolarNode>(),
        new NodeCreator<GaussianNode>()};

        public static Node Create(int i)
        {
            return nodes[i].Create();
        }
        public static Node Create(NEAT.GENES.ACTIVATION activation)
        {
            return nodes[(int)activation].Create();
        }
    }
}
