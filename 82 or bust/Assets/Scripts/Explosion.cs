using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Hitbox
{
    [SerializeField] int trauma;
    new void Start()
    {
        CameraManager.SetTrauma(trauma);
        base.Start();
        Activate(2);
        Destroy(gameObject, 2);
    }
}
