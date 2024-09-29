using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] float targetRange;

    public static Player player;

    bool hasPOI;
    Vector2 pointOfInterest;

    protected new void Start()
    {
        base.Start();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        HandlePlayerTrackingUpdates();
    }

    void Reinforce(float range)
    {

    }

    #region PLAYER_TRACKING

    protected bool playerVisible;
    protected bool PlayerTargetable()
    {
        return playerVisible && Tools.InDistanceToPlayer(trfm.position, targetRange);
    }

    protected bool UpdatePlayerVisible()
    {
        playerVisible = Tools.PlayerVisible(trfm.position);
        return playerVisible;
    }

    int playerTrackingUpdateTimer;
    void HandlePlayerTrackingUpdates()
    {
        if (playerTrackingUpdateTimer > 0) { playerTrackingUpdateTimer--; }
        else
        {
            playerTrackingUpdateTimer = 15;
            UpdatePlayerVisible();
        }
    }

    #endregion
}
