using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MobileEntity
{
    public static Transform trfm;
    public static Player self;

    private void Awake()
    {
        trfm = transform;
        self = GetComponent<Player>();
    }

    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
