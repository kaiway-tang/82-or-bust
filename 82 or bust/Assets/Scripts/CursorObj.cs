using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObj : MonoBehaviour
{
    public static Transform trfm;
    // Start is called before the first frame update
    void Awake()
    {
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
    }
}
