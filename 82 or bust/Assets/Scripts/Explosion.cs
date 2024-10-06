using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Hitbox
{
    new void Start()
    {
        base.Start();
        Activate(5);
        Destroy(gameObject, 2);
    }
}
