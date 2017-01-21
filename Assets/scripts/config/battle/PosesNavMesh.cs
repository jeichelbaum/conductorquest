using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoseNode
{
    public GameObject pose;
    public int[] choices;
}

public class PosesNavMesh : MonoBehaviour {


    public int debugNode = 0;
    PoseNode[] nodes;

    public PoseNode topleft;
    public PoseNode topmid;
    public PoseNode topright;

    public PoseNode midleft;
    public PoseNode midright;

    public PoseNode bottomleft;
    public PoseNode bottommid;
    public PoseNode bottomright;

    void InitNodesArray()
    {
        nodes = new PoseNode[] {
            topleft, topmid, topright,
            midleft, null, midright,
            bottomleft, bottommid, bottomright
        };
    }

    void OnValidate()
    {
        InitNodesArray();
    }

    public PoseNode GetNode(int index)
    {
        return nodes[index - 1];
    }

    void OnDrawGizmosSelected()
    {
        if (debugNode < 1 || debugNode > nodes.Length || GetNode(debugNode) == null) return;

        Gizmos.color = new Color(0f, 1f, 0f, .5f);
        Gizmos.DrawSphere(GetNode(debugNode).pose.transform.position, 0.4f);
        foreach (var choice in GetNode(debugNode).choices)
        {
            Gizmos.DrawLine(GetNode(debugNode).pose.transform.position, GetNode(debugNode).pose.transform.position);
        }
    }

    void Start ()
    {
        InitNodesArray();
	}
	
    public int[] GetPossibleNodes(int index)
    {
        return GetNode(index).choices;
    }

    public int GetDistanceToFinisher(int index, int movesLeft)
    {
        List<int> newPossible = new List<int>();
        List<int> possible = new List<int>();
        possible.Add(index);

        for (var i = 0; i < movesLeft; i++)
        {
            foreach(var p in possible)
            {
                if (GetNode(p) != null)
                {
                    foreach (var newp in GetNode(p).choices)
                    {
                        newPossible.Add(newp);
                        if (newp == 8)
                        {
                            return i+1;
                        }
                    }
                }
            }

            possible = newPossible;
            newPossible = new List<int>();
        }

        return -1;
    }
}
