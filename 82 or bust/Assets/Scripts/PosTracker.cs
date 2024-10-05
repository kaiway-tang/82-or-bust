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
            timer = 5;
        }
    }

    void StorePosition()
    {
        positions[ptr] = trfm.position;
        ptr = (ptr + 1) % positions.Length;
    }

    public Vector3 GetPredictedPosition(int ticks)
    {
        return trfm.position;

        ticks = Mathf.Min(ticks, 200);
        int backIndex = ptr - ticks / 10;
        if (backIndex < 0) { backIndex += 20; }
        Vector3 avgVel = (trfm.position - positions[(ptr + 19) % 20]) * 10;
        avgVel += (trfm.position - positions[(ptr + 18) % 20]) * 5;
        avgVel += (trfm.position - positions[(ptr + 16) % 20]) * 1.6f;
        avgVel = avgVel / 3;
        return trfm.position + avgVel * ticks/50f;
    }
}
