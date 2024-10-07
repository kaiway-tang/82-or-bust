using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotor : MobileEntity
{
    [SerializeField] float accl, maxSpeed, friction;
    [SerializeField] int minDist;
    public bool playerVisible;
    [SerializeField] OnGround ledgeDetect;
    [SerializeField] WarpObj warpObj;
    bool ledged;

    Vector3 lastTargetPos;
    [SerializeField] int warpCD, disengagedTimer;

    [SerializeField] Armament armament;

    [SerializeField] int state;
    const int ROAMING = 0, ENGAGED = 1, DISENGAGED = 2, WARPING = 3;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        warpCD = 100;
        playerVisible = false;
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        ledged = ledgeDetect.touchCount < 1;
        
        if (playerVisible != Tools.PlayerVisible(trfm.position))
        {
            playerVisible = !playerVisible;
            if (!playerVisible)
            {
                lastTargetPos = Player.self.trfm.position;
            }
        }
        if (playerVisible)
        {
            if (state == ROAMING || state == DISENGAGED) { state = ENGAGED; }
        }

        ApplyXFriction(friction);

        HandleFacing();
        HandleApproach();
        HandleDisengaged();
        HandleWarping();

        if (warpCD > 0) { warpCD--; }
    }

    void HandleFacing()
    {
        if (state == ENGAGED)
        {
            if (Tools.BoxDist(Player.self.trfm.position, trfm.position) < minDist)
            {
                if (Player.self.trfm.position.x > trfm.position.x) { FaceLeft(); }
                else { FaceRight(); }
            }
            else
            {
                if (Player.self.trfm.position.x > trfm.position.x) { FaceRight(); }
                else { FaceLeft(); }
            }            
        }        
    }

    void HandleApproach()
    {
        if (state == ENGAGED)
        {
            if (ledged)
            {
                ApplyXFriction(friction);
                AddForwardXVelocity(-accl, -maxSpeed);
            }
            if (playerVisible)
            {
                if (!ledged) { AddForwardXVelocity(accl, maxSpeed); }
            }
            else if (!armament.telegraphed && armament.fireTmr < 1)
            {
                disengagedTimer = 150;
                state = DISENGAGED;
            }
        }
    }

    void HandleDisengaged()
    {
        if (state == DISENGAGED)
        {
            disengagedTimer--;

            if (disengagedTimer > 100)
            {
                if (lastTargetPos.x > trfm.position.x) { FaceRight(); }
                else { FaceLeft(); }

                if (ledged)
                {
                    ApplyXFriction(friction * 3);
                }
                else
                {
                    AddForwardXVelocity(accl, maxSpeed);
                }
            }
            else if(warpCD < 1)
            {
                Warp();
            }

            if (disengagedTimer < 1) { state = ROAMING; }
        }
    }

    void Warp()
    {
        warpObj.trfm.position = trfm.position;        
        warpObj.gameObject.SetActive(true);
        warpObj.Activate();
        state = WARPING;
        baseObj.SetActive(false);
    }

    int warpTimer;
    void HandleWarping()
    {
        if (warpTimer > 0)
        {
            warpTimer--;
        }
    }

    public void EndWarp()
    {
        if (!trfm) { return; }

        vect3 = warpObj.trfm.position;
        vect3.z = 0;
        trfm.position = vect3;

        baseObj.SetActive(true);
        state = ROAMING;
        warpCD = 100;
    }
}
