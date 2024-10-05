using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MobileEntity
{
    public static Player self;

    [SerializeField] float groundedFriction, aerialFriction;
    [SerializeField] float groundedAcceleration, aerialAcceleration, maxSpeed;

    [SerializeField] float jumpPower, doubleJumpPower;
    bool hasDJump;

    int mana;
    [SerializeField] int dashPower, dashMovementDuration, dashIFrameDuration, dashExitVelocity;
    [SerializeField] int dslashPower, dslashMovementDuration, dslashIFrameDuration, dslashExitVelocity;
    [SerializeField] Hitbox dslashHitbox;
    [SerializeField] GameObject perfectDodgeObj;

    [SerializeField] Scaler manaScaler;
    [SerializeField] Collider2D hurtbox;

    [SerializeField] GameObject dJumpFX;

    Vector3 mousePos;
    [SerializeField] PosTracker posTracker;

    private void Awake()
    {
        trfm = transform;
        self = GetComponent<Player>();
        Enemy.player = self;
        GameManager.playerPosTracker = posTracker;
        Tools.playerTrfm = trfm;
    }

    new void Start()
    {
        base.Start();

        AddMana(1000);
    }

    private void Update()
    {
        HandleJump();
        DashSlashCast();
        DashRollCast();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;

        HandleFacing();
        HandleFriction();
        HandleHorizontalMovement();

        HandleDSlash();
        HandleDashRoll();
        HandleMana();
    }

    #region MOVEMENT

    [SerializeField] int movementLock;

    void HandleHorizontalMovement()
    {
        if (movementLock > 0) { return; }

        bool isMoving = false;
        if (In.LeftHeld())
        {
            if (!In.RightHeld())
            {
                if (IsTouchingGround())
                {
                    AddXVelocity(-groundedAcceleration, -maxSpeed);
                    isMoving = true;
                }
                else
                {
                    AddXVelocity(-aerialAcceleration, -maxSpeed);
                }
            }
        }
        else if (In.RightHeld())
        {
            if (IsTouchingGround())
            {
                AddXVelocity(groundedAcceleration, maxSpeed);
                isMoving = true;
            }
            else
            {
                AddXVelocity(aerialAcceleration, maxSpeed);
            }
        }
    }

    void HandleJump()
    {
        if (In.JumpPressed())
        {
            if (IsTouchingGround())
            {
                SetYVelocity(jumpPower);
            }
            else if (hasDJump)
            {
                Instantiate(dJumpFX, trfm.position + Vector3.up * -.6f, Quaternion.identity);
                SetYVelocity(doubleJumpPower);
                hasDJump = false;
            }
        }
    }

    void HandleFriction()
    {
        if (dashMovementTmr > 0) { return; }
        if (IsTouchingGround())
        {
            ApplyXFriction(groundedFriction);
            hasDJump = true;
        }
        else
        {
            ApplyXFriction(aerialFriction);
        }
    }

#endregion

    void HandleFacing()
    {
        if (trfm.position.x < mousePos.x)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }

    #region ABILITIES

    public void PerfectDodged()
    {
        GameManager.SetSloMo(0.5f);
        AddMana(500);
    }

    void HandleMana()
    {
        if (mana < 1000)
        {
            AddMana(5);
        }
    }

    void AddMana(int amount)
    {
        mana += amount;
        if (mana > 1000) { mana = 1000; }
        manaScaler.SetTargetScale(mana * 0.001f, 0.3f);
    } 

    int dslashMovementTmr, dslashIFrameTmr;
    void DashSlashCast()
    {
        if (In.DSlashPressed() && mana >= 500)
        {
            if (dashIFrameTmr > 0) { dashIFrameTmr = 1; }
            if (dashMovementTmr > 0) { dashMovementTmr = 1; }
            if (dashIFrameTmr > 0 || dashMovementTmr > 0) { HandleDashRoll(); }

            if (dslashIFrameTmr < 1) { ToggleInvuln(true); }
            if (dslashMovementTmr < 1) { movementLock++; }

            rb.gravityScale = 0;
            dslashMovementTmr = dslashMovementDuration;
            dslashIFrameTmr = dslashIFrameDuration; 

            rb.velocity = (mousePos - trfm.position).normalized * dslashPower;

            dslashHitbox.Activate(dslashMovementTmr);
            dslashHitbox.trfm.right = rb.velocity;

            mana -= 500;
            manaScaler.SetTargetScale(mana * 0.001f, 0.3f);
        }
    }

    void HandleDSlash()
    {
        if (dslashMovementTmr > 0)
        {
            dslashMovementTmr--;
            if (dslashMovementTmr < 1)
            {
                rb.velocity = rb.velocity * dslashExitVelocity / dslashPower;
                rb.gravityScale = 6;
                movementLock--;
            }
        }
        
        if (dslashIFrameTmr > 0)
        {
            dslashIFrameTmr--;
            if (dslashIFrameTmr < 1) { ToggleInvuln(false); }
        }
    }

    int dashMovementTmr, dashIFrameTmr;
    void DashRollCast()
    {
        if (In.DashRollPressed() && mana >= 250 && dashMovementTmr < 1)
        {
            if (dashIFrameTmr < 1) { ToggleInvuln(true); }
            if (dashMovementTmr < 1) { movementLock++; }

            rb.gravityScale = 0;
            dashMovementTmr = dashMovementDuration;
            dashIFrameTmr = dashIFrameDuration;
            
            rb.velocity = (mousePos - trfm.position).normalized * dashPower;

            Instantiate(perfectDodgeObj, trfm.position, Quaternion.identity);

            mana -= 250;
            manaScaler.SetTargetScale(mana * 0.001f, 0.3f);
        }
    }
    void HandleDashRoll()
    {
        if (dashMovementTmr > 0)
        {
            ApplyDirectionalFriction((dashPower - dashExitVelocity)/dashMovementDuration);
            dashMovementTmr--;
            if (dashMovementTmr == 0)
            {
                //rb.velocity = rb.velocity * dashExitVelocity / rb.velocity.magnitude;
                rb.gravityScale = 6;
                movementLock--;
            }
        }

        if (dashIFrameTmr > 0)
        {
            dashIFrameTmr--;
            if (dashIFrameTmr == 0)
            {
                ToggleInvuln(false);
            }
        }
    }

    int invuln;
    void ToggleInvuln(bool active)
    {
        if (active) { invuln++; hurtbox.enabled = false; }
        else
        {
            invuln--;
            if (invuln < 1) { hurtbox.enabled = true; }
        }
    }

    #endregion

    public override int TakeDamage(int amount, int sourceID)
    {
        if (sourceID != 0 && sourceID == entityID) { return IGNORED; }

        CameraManager.SetDmgVig(Mathf.Min(1.0f, amount/30f));
        CameraManager.SetTrauma(Mathf.Min(30, amount));

        int result = base.TakeDamage(amount, sourceID);
        if (result == DEAD)
        {
            baseObj.SetActive(false);
        }

        return result;
    }
}
