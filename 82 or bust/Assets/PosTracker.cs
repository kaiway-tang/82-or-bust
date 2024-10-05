using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosTracker : MonoBehaviour
{
    int timer;
    int ptr;
    [SerializeField] Vector3[] positions;
    Transform trfm;
    
    void Start()
    {
        trfm = transform;
        positions = new Vector3[20];
    }

    
    void FixedUpdate()
    {
        if (timer > 0) { timer--; }
        else
        {
            StorePosition();
            timer = 10;
        }
    }

    void StorePosition()
    {
        positions[ptr] = trfm.position;
        ptr = (ptr + 1) % positions.Length;
    }

    public Vector3 GetPredictedPosition(int ticks)
    {
        ticks = Mathf.Min(ticks, 200);
        int backIndex = ptr - ticks / 10;
        if (backIndex < 0) { backIndex += 20; }
        return trfm.position + (trfm.position - positions[(ptr + 1)%20]) * ticks/200;
    }
}
