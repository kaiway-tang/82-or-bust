using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject grid;
    public GameObject[] chunks;

    public static LevelManager self;
    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
