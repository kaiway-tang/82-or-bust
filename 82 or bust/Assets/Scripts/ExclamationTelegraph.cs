using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationTelegraph : MonoBehaviour
{
    [SerializeField] Vector3 rise;
    [SerializeField] Transform trfm;

    private void Start()
    {
        Destroy(gameObject, .7f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += rise;
    }
}