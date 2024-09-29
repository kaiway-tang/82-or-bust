using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileEntity : HPEntity
{
    public Rigidbody2D rb;
    public Transform trfm;

    protected void Awake()
    {
        if (!rb) { rb = GetComponent<Rigidbody2D>(); }
        if (!trfm) { trfm = transform; }
    }

    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
