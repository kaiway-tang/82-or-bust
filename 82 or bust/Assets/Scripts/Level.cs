using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    [SerializeField] Transform[] nodes;
    [SerializeField] GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void Load()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            GameObject newChunk =  Instantiate(LevelManager.self.chunks[Random.Range(0, LevelManager.self.chunks.Length)],
                nodes[i].position, Quaternion.identity);

            newChunk.transform.parent = LevelManager.self.grid.transform;
            newChunk.AddComponent<TilemapCollider2D>();

            Chunk chunk = newChunk.GetComponent<Chunk>();
            for (int j = 0; j < chunk.spawnNodes.Length; j++)
            {
                if (Random.Range(0, chunk.spawnNodes.Length) < 3)
                { Instantiate(enemies[Random.Range(0, enemies.Length)], chunk.spawnNodes[j].position, Quaternion.identity); }
                Destroy(chunk.spawnNodes[j].gameObject);
            }
        }
    }
}
