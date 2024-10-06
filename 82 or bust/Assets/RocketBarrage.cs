using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBarrage : Armament
{
    [SerializeField] GameObject rocket;
    new void Start()
    {
        base.Start();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (fireTmr > 0)
        {
            fireTmr--;
            if (fireTmr % 25 == 0)
            {
                Instantiate(rocket, firepoint.position, firepoint.rotation);
            }
        }

        if (playerVisible)
        {
            Aim();
        }
    }

    protected override void Fire()
    {
        base.Fire();
        fireTmr = 75;
    }

    protected override void Telegraph()
    {
        base.Telegraph();
    }
}
