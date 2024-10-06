using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : Armament
{
    [SerializeField] GameObject bullet, aimLine;
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
            if (fireTmr % 10 == 0)
            {
                Instantiate(bullet, firepoint.position, firepoint.rotation);
            }
            if (fireTmr < 1)
            {
                aimLine.SetActive(false);
                positionLocked = false;
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
        fireTmr = 50;
    }

    protected override void Telegraph()
    {
        base.Telegraph();
        aimLine.SetActive(true);
        positionLocked = true;
    }
}
