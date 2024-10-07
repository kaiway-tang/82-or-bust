using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    // player transform: GameManager.playerTrfm  OR  player.trfm

    [SerializeField] GameObject damageFX, nanobot;
    [SerializeField] float targetRange;
    [SerializeField] Armament armament;
    [SerializeField] int damageTrauma, deathTrauma;

    int difficulty;
    public static Player player;

    [SerializeField] GameObject inactiveShell;

    [SerializeField] int score;

    bool hasPOI;
    Vector2 pointOfInterest;

    protected new void Start()
    {
        base.Start();
        if (damageTrauma == 0) { damageTrauma = 12;}
        if (deathTrauma == 0) { deathTrauma = 18; }
        difficulty = GameManager.self.difficulty;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        HandlePlayerTrackingUpdates();
        if (GameManager.self.difficulty > difficulty)
        {
            Destroy(baseObj);
        }
    }

    public override int TakeDamage(int amount, int sourceID)
    {
        int result = base.TakeDamage(amount, sourceID);
        if (result != HPEntity.IGNORED)
        {
            Instantiate(damageFX, trfm.position, Quaternion.identity);
            if (result == HPEntity.DEAD)
            {
                CameraManager.SetTrauma(deathTrauma);
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(nanobot, trfm.position, Quaternion.identity);
                }
                Instantiate(inactiveShell, trfm.position, Quaternion.identity);

                GameManager.self.score += score;
                Destroy(baseObj);
            }
            else
            {
                CameraManager.SetTrauma(damageTrauma);
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
