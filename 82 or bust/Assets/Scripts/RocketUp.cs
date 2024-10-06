using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketUp : Armament
{
    [SerializeField] GameObject rocket;
    new void Start()
    {
        base.Start();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Fire()
    {
        base.Fire();
        Instantiate(rocket, firepoint.position, firepoint.rotation);
    }

    protected override void Telegraph()
    {
        base.Telegraph();
    }
}
