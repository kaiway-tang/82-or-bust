using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotor : MobileEntity
{
    [SerializeField] float accl, maxSpeed, friction;
    public bool playerVisible;
    [SerializeField] OnGround ledgeDetect;
    [SerializeField] WarpObj warpObj;
    bool ledged;

    Vector3 lastTargetPos;
    [SerializeField] int warpCD, disengagedTimer;

    [SerializeField] int state;
    const int ROAMING = 0, ENGAGED = 1, DISENGAGED = 2, WARPING = 3;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
        if (playerVisible)
        {
            if (Player.self.trfm.position.x > trfm.position.x) { FaceRight(); }
            else { FaceLeft(); }
        }
    }

    void HandleApproach()
    {
        if (state == ENGAGED)
        {
            if (ledged)
            {
                ApplyXFriction(friction * 3);
            }
            if (playerVisible)
            {
                if (!ledged) { AddForwardXVelocity(accl, maxSpeed); }                
            }
            else
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
        state = ROAMING;
        warpCD = 100;
    }
}
