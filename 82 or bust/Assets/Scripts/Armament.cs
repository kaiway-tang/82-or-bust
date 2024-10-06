using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armament : MonoBehaviour
{
    [SerializeField] protected int cdMin, cdMax, baseCd, telegraphTime;
    [SerializeField] protected Transform trfm, firepoint;
    [SerializeField] protected float aimRate;
    protected int cooldown;
    public bool playerVisible, telegraphed;
    bool lastPlayerVisible;

    protected bool positionLocked;
    public bool LockPosition()
    {
        return positionLocked;
    }

    protected void Start()
    {
        cooldown = baseCd;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (lastPlayerVisible != playerVisible)
        {
            lastPlayerVisible = playerVisible;
            if (lastPlayerVisible && cooldown > telegraphTime)
            {
                cooldown = baseCd;
            }
        }

        if (playerVisible || cooldown <= telegraphTime)
        {
            cooldown--;
            if (cooldown == telegraphTime)
            {
                Telegraph();
            }
            if (cooldown < 1)
            {
                Fire();
                cooldown = Random.Range(cdMin, cdMax);
            }
        }
    }

    protected void Aim()
    {
        Tools.FacePosition(trfm, Player.self.trfm.position, aimRate);
    }

    protected int fireTmr;
    protected virtual void Fire()
    {
        telegraphed = false;
    }

    protected virtual void Telegraph()
    {
        telegraphed = true;
        Instantiate(GameManager.exclamation, trfm.position, Quaternion.identity);
    }
}
