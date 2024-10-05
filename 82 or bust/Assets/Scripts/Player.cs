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
    [SerializeField] Collider2D hurtbox;

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

        mana = 999999;
    }

    private void Update()
    {
        HandleJump();
        DashRollCast();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleFacing();
        HandleFriction();
        HandleHorizontalMovement();

        HandleDashRoll();
    }

    #region MOVEMENT

    int movementLock;
    [SerializeField] bool lheld, rheld, grounded;

    void HandleHorizontalMovement()
    {
        if (movementLock > 0) { return; }

        lheld = In.LeftHeld();
        rheld = In.RightHeld();
        grounded = IsTouchingGround();

        bool isMoving = false;
        if (In.LeftHeld())
        {
            Debug.Log("left");
            if (!In.RightHeld())
            {
                Debug.Log("not right");
                if (IsTouchingGround())
                {
                    Debug.Log("grounded");
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

    int dashMovementTmr, dashIFrameTmr;
    void DashRollCast()
    {
        if (In.DashRollPressed() && mana >= 25)
        {
            rb.gravityScale = 0;
            dashMovementTmr = dashMovementDuration;
            dashIFrameTmr = dashIFrameDuration;
            hurtbox.enabled = false;

            Vector3 dashVect = (mousePos - trfm.position).normalized * dashPower;
            Vector3 rbVect = rb.velocity;
            if ((dashVect + rbVect).sqrMagnitude > dashVect.sqrMagnitude)
            {
                rb.velocity = dashVect + rbVect;
            }
            else
            {
                rb.velocity = dashVect;
            }
            movementLock++;

            mana -= 25;
        }
    }
    void HandleDashRoll()
    {
        if (dashMovementTmr > 0)
        {
            dashMovementTmr--;
            if (dashMovementTmr == 0)
            {
                rb.velocity = rb.velocity * dashExitVelocity / rb.velocity.magnitude;
                rb.gravityScale = 6;
                movementLock--;
            }
        }

        if (dashIFrameTmr > 0)
        {
            dashIFrameTmr--;
            if (dashIFrameTmr == 0)
            {
                hurtbox.enabled = true;
            }
        }
    }

    #endregion
}
