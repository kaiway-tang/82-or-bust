using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    public Tilemap tilemap;
    public List<GameObject> spawnNodes;
    public List<Bridge> bridges;

    public void Init()
    {
        return;
        spawnNodes = new List<GameObject>();
        bridges = new List<Bridge>();
        int numChild = transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            if (transform.GetChild(i).tag == "spawnNode")
            {
                spawnNodes.Add(transform.GetChild(i).gameObject);
            }
            else if (transform.GetChild(i).tag == "bridge")
            {
                bridges.Add(transform.GetChild(i).GetComponent<Bridge>());
            }
        }
    }
}
