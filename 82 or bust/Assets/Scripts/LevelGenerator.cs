using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> chunks;
    NavMeshSurface navmesh;

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshSurface>();
    }

    void GenerateLevel(int width, int height)
    {
        /*
         * 
         */
        if (chunks.Count == 0)
        {
            Debug.LogError("LevelGenerator: No chunks provided for generation!");
            return;
        }

        navmesh.BuildNavMeshAsync(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
