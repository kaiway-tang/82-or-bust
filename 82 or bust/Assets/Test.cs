using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] PosTracker postrack;
    
    void FixedUpdate()
    {
        transform.position = postrack.GetPredictedPosition(50);
    }
}
