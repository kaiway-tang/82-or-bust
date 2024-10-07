using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveShell : MonoBehaviour
{
    public Transform trfm;
    public GameObject enemyObj;
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.self.NewShell(GetComponent<InactiveShell>());
    }

    public void CollectNanobot()
    {
        count++;
        if (count > 1) { Activate(); }
    }

    public void Activate()
    {
        Instantiate(enemyObj, trfm.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
