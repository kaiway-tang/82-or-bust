using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(NavMeshSurface))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] bridge;
    [SerializeField] Tilemap curTilemap;
    [SerializeField] GameObject navMeshPrefab;
    [SerializeField] int chunkWidth = 15;
    [SerializeField] int chunkHeight = 10;
    NavMeshSurface navmesh;
    GameObject navMeshObj;
    List<GameObject> chunks;
    int levelAnchorx = 0;
    int levelAnchory = 0; 

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshSurface>();
        chunks = new List<GameObject>();
        LoadChunks();
        GenerateLevel(3);
    }

    void LoadChunks()
    {
        // Load all prefabs from the specified folder inside the Resources folder
        GameObject[] loadedChunks = Resources.LoadAll<GameObject>("Chunks");

        // Add them to the chunks list
        foreach (var chunk in loadedChunks)
        {
            chunks.Add(chunk);
            Debug.Log("Loaded chunk: " + chunk.name);

        }

        if (chunks.Count == 0)
        {
            Debug.LogWarning("LevelGenerator: No chunks found in Resources/Chunks!");
        }
    }

    void LoadSpawnpoints(GameObject chunkObj, Vector3 anchor)
    {
        //GameObject newChunk = Instantiate(chunkObj, center, Quaternion.identity);

        //newChunk.transform.parent = LevelManager.self.grid.transform;
        //newChunk.AddComponent<TilemapCollider2D>();

        Chunk chunk = chunkObj.GetComponent<Chunk>();
        chunk.Init();
        for (int j = 0; j < chunk.spawnNodes.Count; j++)
        {
            if (Random.Range(0, chunk.spawnNodes.Count) < 3)
            { Instantiate(enemies[Random.Range(0, enemies.Length)], anchor + chunk.spawnNodes[j].transform.localPosition, Quaternion.identity); }
            // Destroy(chunk.spawnNodes[j].gameObject);
        }

        for (int i = 0; i < chunk.bridges.Count; i++)
        {
            if (Random.Range(0, 2) < 99)
            {
                if (chunk.bridges[i].width == 0)
                {
                    Debug.Log("bruh: " + chunk.bridges[i]);
                    Instantiate(bridge[3], anchor + chunk.bridges[i].trfm.localPosition, Quaternion.identity);
                    continue;
                }                
                Instantiate(bridge[chunk.bridges[i].width - 2], anchor + chunk.bridges[i].trfm.localPosition, Quaternion.identity);
            }
        }
    }

    void GenerateLevel(int size)
    {
        curTilemap.ClearAllTiles();
        for (int j = 0; j < size; ++j)
        {
            for (int i = 0; i < size; ++i)
            {
                GenerateCell(levelAnchorx + i * chunkWidth, levelAnchory + j * chunkHeight);
            }
        }
        // Generate the navigation area accordingly
        // NOTE: We assume one cell in a grid is 1 unit wide and 1 unit high
        Vector3 coords = curTilemap.GetCellCenterWorld(new Vector3Int(levelAnchorx, levelAnchory, 0));
        float middlex = coords.x - 0.5f + (float)size / 2 * chunkWidth;  // -.5 to get left edge of cell's coords
        float middley = coords.y - 0.5f + (float)size / 2 * chunkHeight;  // same with bot edge
        if (!navMeshObj)
        {
            navMeshObj = Instantiate(navMeshPrefab, Vector2.zero, Quaternion.identity);
        }
        navMeshObj.transform.position = new Vector3(middlex, middley);
        navMeshObj.transform.localScale = new Vector3((float)size * chunkWidth, (float)size * chunkHeight);
        navmesh.BuildNavMeshAsync();

        levelAnchorx += size * chunkWidth;
        levelAnchory += size * chunkHeight;
    }

    void GenerateCell(int anchorx, int anchory)
    {
        // Use below function when system linked
        // Tilemap refMap = LeelManager.self.chunks[Random.Range(0, LevelManager.self.chunks.Length)].GetComponent<Tilemap>();
        Chunk chunk = chunks[Random.Range(0, chunks.Count)].GetComponent<Chunk>();
        Tilemap refMap = chunk.tilemap;
        refMap.CompressBounds();
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                Vector3Int chunkPosition = new Vector3Int(refMap.origin.x + x, refMap.origin.y + y, refMap.origin.z);
                Vector3Int tilePosition = new Vector3Int(anchorx + x, anchory + y, 0);

                TileBase tile = refMap.GetTile(chunkPosition);

                if (tile != null)
                {
                    curTilemap.SetTile(tilePosition, tile);
                }
            }
        }

        // Load spawnpoints
        LoadSpawnpoints(refMap.gameObject, new Vector3(anchorx, anchory));

        // SpawnEnemies(chunk, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
