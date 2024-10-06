using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    [SerializeField] float start, dest, lerp;
    [SerializeField] float threeZ;
    [SerializeField] Transform one, two, three;
    private void FixedUpdate()
    {
        start = Tools.RotationalLerp(start, dest, lerp);

        Tools.FacePosition(one, two.position, lerp);

    }
}
