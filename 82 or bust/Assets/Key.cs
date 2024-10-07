using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Gate gate;
    public Vector3 hidingPoint;
    int state = 0;
    const int START = 0, HIDING = 1, IDLE = 2, COLLECTING = 3;
    [SerializeField] float collectSpeed;
    [SerializeField] GameObject breakWall;
    Transform trfm;

    void Start()
    {
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == START)
        {
            if (!breakWall)
            {
                state = HIDING;
                //set hidingPoint (vector 3)
                hidingPoint = FindHidingSpot(); 
            }
        }
        if (state == HIDING)
        {
            trfm.position += (hidingPoint - trfm.position).normalized * collectSpeed;
            if (Tools.BoxDist(trfm.position, hidingPoint) < 1)
            {
                state = IDLE;
            }
        }
        if (state == IDLE)
        {
            if (Tools.BoxDist(trfm.position, Player.self.trfm.position) < 2)
            {
                state = COLLECTING;
            }
        }
        if (state == COLLECTING)
        {
            trfm.position += (gate.trfm.position - trfm.position).normalized * collectSpeed;
            if (Tools.BoxDist(trfm.position, gate.trfm.position) < 1)
            {
                gate.Open();
                Destroy(gameObject);
            }
        }
    }

    Vector3 FindHidingSpot()
    {
        // Choose from furthest half from the key 
        LevelGenerator.Instance.SpawnPositions.Sort(delegate(Vector3 x, Vector3 y)
        {
            if (Vector3.SqrMagnitude(x - transform.position) < Vector3.SqrMagnitude(y - transform.position))
            {
                return 1;
            } else
            {
                return -1;
            }
        }
        );
        return LevelGenerator.Instance.SpawnPositions[Random.Range(0, LevelGenerator.Instance.SpawnPositions.Count / 2)];
    }
}
