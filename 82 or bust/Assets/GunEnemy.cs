using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : SmartEnemy
{
    [SerializeField] Transform gunTrfm;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentState == EnemyStates.Idle) { HandleIdle(); }
        else
        {

        }
    }

    void HandleIdle()
    {

    }

    void HandleAiming()
    {
        //Tools.LerpRotation(gunTrfm, )
    }
}
