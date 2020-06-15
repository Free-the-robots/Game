using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEvoWeapons : MonoBehaviour
{
    public NEAT.Person behaviour;
    public GameObject nodeObj;

    private Dictionary<int, Vector3> nodePair = new Dictionary<int, Vector3>();
    private Dictionary<int, int> countLvl = new Dictionary<int, int>();
    // Start is called before the first frame update
    void OnEnable()
    {
        if (behaviour.network == null)
            behaviour.buildModel();
        BuildEvoWeapon();
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

            DrawNodeChildren(net.inNodes[i], pos, this.transform, 1);
        }
        nodePair.Clear();
        countLvl.Clear();
    }

    private void CountNodesPerLevel(NN.Node node, int lvl)
    {
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

    private void DrawNodeChildren(NN.Node node, Vector3 position, Transform parent, int lvl)
    {
        nodePair[node.nb] = position;
        GameObject obj = GameObject.Instantiate(nodeObj);
        obj.transform.SetParent(parent);
        obj.transform.position = position;

        if (node.outNodes != null && node.outNodes.Count > 0)
        {
            for (int i = 0; i < node.outNodes.Count; i++)
            {
                float y = 0f;
                if(node.outNodes.Count > 1)
                    y = ((float)i) / (node.outNodes.Count-1) - 0.5f;
                Vector3 pos = new Vector3(position.x + 2f, y * countLvl[lvl], 0f);
                if (nodePair.ContainsKey(node.outNodes[i].nb))
                    pos = nodePair[node.outNodes[i].nb];
                else
                    DrawNodeChildren(node.outNodes[i], pos, parent,lvl+1);
                Debug.DrawLine(position, pos,Color.white,100f);
            }
        }
    }
}
