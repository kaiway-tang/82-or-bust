using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : SmartEnemy
{
    [SerializeField] Transform gunTrfm, firepoint;
    [SerializeField] GameObject bullet;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        currentState = EnemyStates.Approach;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentState == EnemyStates.Idle) { HandleIdle(); }
        else
        {
            HandleAiming();
        }
    }

    void HandleIdle()
    {

    }

    int cooldown;
    void HandleAiming()
    {
        Tools.LerpRotation(gunTrfm, player.trfm.position, .2f);

        cooldown--;

        if (cooldown == 75) { Instantiate(GameManager.exclamation, trfm.position, Quaternion.identity); }

        if (cooldown < 50 && cooldown % 10 == 0)
        {
            Instantiate(bullet, firepoint.position, gunTrfm.rotation);
            if (cooldown < 1) { cooldown = Random.Range(100, 125); }
        }
    }
}
