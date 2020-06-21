using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEvoWeapons : MonoBehaviour
{
    public NEAT.Person behaviour;
    public GameObject nodeObj;
    public GameObject nodeTransitionObj;
    public Material lineMaterial;

    private Dictionary<Transform, List<Transform>> inNOut = new Dictionary<Transform, List<Transform>>();
    private Dictionary<int, Transform> nodePair = new Dictionary<int, Transform>();
    private Dictionary<int, int> countLvl = new Dictionary<int, int>();
    private int lvlMax = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        BuildEvoWeapon();
    }

    private void OnDisable()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuildEvoWeapon()
    {
        if (behaviour.network == null)
            behaviour.buildModel();
        NN.Net net = behaviour.network;

        nodePair.Clear();
        countLvl.Clear();
        for (int i = 0; i < net.inNodes.Count; i++)
        {
            CountNodesPerLevel(net.inNodes[i], 1);
        }
        for (int i = 0; i < net.inNodes.Count; i++)
        {
            float y = 0f;
            if (net.outNodes.Count > 1)
                y = ((float)i) / (net.inNodes.Count - 1) - 0.5f;
            Vector3 pos = new Vector3(0f, y * 4f, 0f);

            DrawNodeChildren(net.inNodes[i], pos, this.transform, 1, null);
        }
        nodePair.Clear();
        countLvl.Clear();

        ceateTransitionNodes();
    }

    private void CountNodesPerLevel(NN.Node node, int lvl)
    {
        if (lvlMax > lvl)
            lvlMax = lvl;
        if (node.outNodes != null && node.outNodes.Count > 0)
        {
            for (int i = 0; i < node.outNodes.Count; i++)
            {
                if (countLvl.ContainsKey(lvl))
                    countLvl[lvl]++;
                else
                    countLvl[lvl] = 1;
                CountNodesPerLevel(node.outNodes[i], lvl+1);
            }
        }
    }

    private void DrawNodeChildren(NN.Node node, Vector3 position, Transform parent, int lvl, Transform before)
    {
        GameObject obj = GameObject.Instantiate(nodeObj);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        nodePair[node.nb] = obj.transform;

        if (before != null)
        {
            if (!inNOut.ContainsKey(before) || inNOut[before] == null)
                inNOut[before] = new List<Transform>();

            inNOut[before].Add(obj.transform);
        }

        obj.GetComponent<SpriteRenderer>().color = getColorFromType(node);

        if (node.outNodes != null && node.outNodes.Count > 0)
        {
            for (int i = 0; i < node.outNodes.Count; i++)
            {
                float y = 0f;
                if(node.outNodes.Count > 1)
                    y = ((float)i) / (node.outNodes.Count-1) - 0.5f;
                Vector3 pos = new Vector3(position.x + 2f, y * countLvl[lvl], 0f);

                if (nodePair.ContainsKey(node.outNodes[i].nb))
                {
                    pos = nodePair[node.outNodes[i].nb].position;
                    if (obj.transform != null)
                    {
                        if (!inNOut.ContainsKey(obj.transform) || inNOut[obj.transform] == null)
                            inNOut[obj.transform] = new List<Transform>();

                        inNOut[obj.transform].Add(nodePair[node.outNodes[i].nb]);
                    }
                }
                else
                    DrawNodeChildren(node.outNodes[i], pos, parent,lvl+1, obj.transform);

                //Debug.DrawLine(position, pos,Color.white,100f);
                createTrail(node, position, pos, node.outW[i]);
            }
        }
    }

    private Color getColorFromType(NN.Node node)
    {
        Color res = Color.white;
        switch (node)
        {
            case NN.Rectifier n:
                res = new Color(0f,0f,1f);
                break;
            case NN.SinNode n:
                res = new Color(1f, 0.6f, 0.6f);
                break;
            case NN.CosNode n:
                res = new Color(0.6f, 1f, 0.6f);
                break;
            case NN.TanhNode n:
                res = new Color(1f, 1f, 0.6f);
                break;
            case NN.GaussianNode n:
                res = new Color(0.6f, 0.6f, 1f);
                break;
            case NN.BipolarNode n:
                res = new Color(1f, 0f, 1f);
                break;
            case NN.TanNode n:
                res = new Color(1f, 1f, 0f);
                break;
            case null:
                res = Color.white;
                break;
        }

        if (node.outNodes == null || node.outNodes.Count == 0)
            res = Color.red;
        if (node.inNodes == null || node.inNodes.Count == 0)
            res = Color.blue;

        return res;
    }

    private GameObject createTrail(NN.Node node, Vector3 positionStart, Vector3 positionEnd, float weight)
    {
        GameObject trail = new GameObject();
        trail.transform.SetParent(transform);
        trail.AddComponent<LineRenderer>();
        trail.transform.position = Vector3.zero;

        trail.GetComponent<LineRenderer>().sharedMaterial = lineMaterial;

        trail.GetComponent<LineRenderer>().SetPosition(0, positionStart);
        trail.GetComponent<LineRenderer>().SetPosition(1, positionEnd);

        float w = weight * 0.04f;
        if (w < 0.01f)
            w = 0.01f;
        if (w > 0.2f)
            w = 0.2f;

        trail.GetComponent<LineRenderer>().startWidth = w;
        trail.GetComponent<LineRenderer>().endWidth = w;

        return trail;
    }

    private void ceateTransitionNodes()
    {
        foreach(KeyValuePair<Transform, List<Transform>> pairs in inNOut)
        {
            for(int i = 0; i < pairs.Value.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(nodeTransitionObj);
                obj.transform.SetParent(transform);
                obj.GetComponent<LerpToWhenEnabled>().from = pairs.Key;
                obj.GetComponent<LerpToWhenEnabled>().to = pairs.Value[i];
                obj.GetComponent<LerpToWhenEnabled>().enabled = true;
            }
        }
    }
}
