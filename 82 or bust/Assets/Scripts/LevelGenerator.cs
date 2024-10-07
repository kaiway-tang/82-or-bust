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
    [SerializeField] RuleTile borderTile;
    [SerializeField] GameObject navMeshPrefab;
    [SerializeField] GameObject breakRoomPrefab;
    [SerializeField] int breakRoomWidth = 10;
    [SerializeField] int breakRoomHeight = 10;
    [SerializeField] int chunkWidth = 15;
    [SerializeField] int chunkHeight = 10;
    NavMeshSurface navmesh;
    GameObject navMeshObj;
    GameObject breakRoomObj;
    List<GameObject> chunks;
    List<GameObject> startChunks;
    List<GameObject> endChunks;
    int levelAnchorx = 0;
    int levelAnchory = 0;

    public static LevelGenerator Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        Instance = this;
    }

    void Start()
    {
        navmesh = GetComponent<NavMeshSurface>();
        chunks = new List<GameObject>();
        startChunks = new List<GameObject>();
        endChunks = new List<GameObject>();
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
            if (chunk.CompareTag("EN-EX"))
            {
                startChunks.Add(chunk);
                endChunks.Add(chunk);
            } else if (chunk.CompareTag("EN"))
            {
                startChunks.Add(chunk);
            } else if (chunk.CompareTag("EX"))
            {
                endChunks.Add(chunk);
            }
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

    public void GenerateLevel(int size)
    {
        ClearGeneration();
        // Generate border
        for (int j = 0; j < size * chunkHeight + 5; ++j)
        {
            curTilemap.SetTile(new Vector3Int(levelAnchorx + chunkWidth * size + 1, levelAnchory + j), borderTile);
        }
        for (int j = breakRoomHeight; j < size * chunkHeight + 5; ++j)
        {
            curTilemap.SetTile(new Vector3Int(levelAnchorx, levelAnchory + j), borderTile);
        }
        for (int i = 0; i < size * chunkWidth + 2; ++i)
        {
            curTilemap.SetTile(new Vector3Int(levelAnchorx + i, levelAnchory), borderTile);
        }
        for (int i = 0; i < size * chunkWidth + 2 - breakRoomWidth; ++i)
        {
            curTilemap.SetTile(new Vector3Int(levelAnchorx + i, levelAnchory + chunkHeight * size + 4), borderTile);
        }
        ++levelAnchorx;
        ++levelAnchory;

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

        levelAnchorx += size * chunkWidth + 1;
        levelAnchory += size * chunkHeight + 3;

        // Spawn break room
        // if (breakRoomObj) Destroy(breakRoomObj);  // Note: we don't want to delete break rooms like other stuff
        breakRoomObj = Instantiate(breakRoomPrefab, new Vector3(levelAnchorx, levelAnchory), Quaternion.identity);
    }

    void GenerateCell(int anchorx, int anchory, int requirement = 0)
    {
        // Use below function when system linked
        // Tilemap refMap = LeelManager.self.chunks[Random.Range(0, LevelManager.self.chunks.Length)].GetComponent<Tilemap>();
        Chunk chunk;
        switch (requirement)
        {
            case 1:  // Entrance
                chunk = startChunks[Random.Range(0, chunks.Count)].GetComponent<Chunk>();
                break;
            case 2:  // Exit
                chunk = endChunks[Random.Range(0, chunks.Count)].GetComponent<Chunk>();
                break;
            default:  // Normal
                chunk = chunks[Random.Range(0, chunks.Count)].GetComponent<Chunk>();
                break;

        }
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

    void ClearGeneration()
    {
        curTilemap.ClearAllTiles();
    }
}
