using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    // player transform: GameManager.playerTrfm  OR  player.trfm


    [SerializeField] GameObject damageFX;
    [SerializeField] float targetRange;
    [SerializeField] Armament armament;

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

    public override int TakeDamage(int amount, int sourceID)
    {
        int result = base.TakeDamage(amount, sourceID);
        if (result != HPEntity.IGNORED)
        {
            Instantiate(damageFX, trfm.position, Quaternion.identity);
            if (result == HPEntity.DEAD)
            {
                Destroy(baseObj);
            }
        }        
        return result;
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
        armament.playerVisible = playerVisible;
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
