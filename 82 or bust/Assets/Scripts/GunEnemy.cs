using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : SmartEnemy
{
    [SerializeField] Transform gunTrfm, firepoint;
    [SerializeField] GameObject bullet, aimLine;
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
        if (cooldown <= 75 || playerVisible) { cooldown--; }

        if (cooldown == 75)
        {
            Instantiate(GameManager.exclamation, trfm.position, Quaternion.identity);
            aimLine.SetActive(true);
        }

        if (cooldown >= 75)
        {
            Tools.FacePosition(gunTrfm, player.trfm.position, .2f);
        }
        if (cooldown < 50)
        {
            if (cooldown % 10 == 0)
            {
                Instantiate(bullet, firepoint.position, gunTrfm.rotation);
                if (cooldown < 1)
                {
                    cooldown = Random.Range(150, 200);
                    aimLine.SetActive(false);
                }
            }
        }
    }
}
