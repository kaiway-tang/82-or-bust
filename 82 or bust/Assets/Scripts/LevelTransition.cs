using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HPEntity ent = collision.GetComponent<HPEntity>();
        if (ent && ent.entityID == 1)
        {
            StartCoroutine(TransitionLevel(collision.transform));
        }
    }

    IEnumerator TransitionLevel(Transform player)
    {
        // Slow mo the player 
        float origScale = Time.timeScale;
        Time.timeScale *= 0.5f;
        // Fade out music, Fade in break room theme

        yield return new WaitForSecondsRealtime(0.5f);
        // Un slow player
        Time.timeScale = origScale;
        // Move player to within break room
        Collider2D playerCol = player.GetComponent<Collider2D>();
        playerCol.enabled = false;
        while (Vector3.Distance(player.position, spawnPoint.position) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position, spawnPoint.position, 5f * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        playerCol.enabled = true;
        // Generate next level (fog off the previous zone)
        LevelGenerator.Instance.GenerateLevel(3); 
    }
}
