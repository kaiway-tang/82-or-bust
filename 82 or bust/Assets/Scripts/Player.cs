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

    Vector2 mousePos;

    private void Awake()
    {
        trfm = transform;
        self = GetComponent<Player>();
    }

    new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        HandleJump();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleFacing();
        HandleFriction();
        HandleHorizontalMovement();
    }

    #region MOVEMENT

    [SerializeField] bool lheld, rheld, grounded;

    void HandleHorizontalMovement()
    {
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
}
