using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] GameObject flamePtclObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] int rate;
    int timer;
    void FixedUpdate()
    {
        if (timer > 0) { timer--; }
        else
        {
            timer = rate;
            Instantiate(flamePtclObj, transform.position, Quaternion.identity);
        }
    }
}
