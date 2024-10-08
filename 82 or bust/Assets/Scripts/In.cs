using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In : MonoBehaviour
{
    public static KeyCode JumpKey, JumpKey1, UpKey, UpKey1, DownKey, DownKey1, LeftKey, LeftKey1, RightKey, RightKey1;
    public static KeyCode
        SlashKey, SlashKey1,
        DSlashKey, DSlashKey1,
        DashRollKey, DashRollKey1;

    public static int slashMB, dslashMB, rollMB;

    static bool firstLoad;

    private void Start()
    {
        if (firstLoad) { return; }

        JumpKey = KeyCode.Space;
        //JumpKey1 = KeyCode.W;

        UpKey = KeyCode.W;
        UpKey1 = KeyCode.UpArrow;

        DownKey = KeyCode.S;
        DownKey1 = KeyCode.DownArrow;

        LeftKey = KeyCode.A;
        LeftKey1 = KeyCode.LeftArrow;

        RightKey = KeyCode.D;
        RightKey1 = KeyCode.RightArrow;

        DashRollKey = KeyCode.LeftShift;
        DashRollKey1 = KeyCode.RightShift;

        slashMB = 1;
        dslashMB = 0;
        rollMB = 1;

        firstLoad = true;
    }

    public bool RebindKey(KeyCode key)
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < 510; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    key = (KeyCode)i;
                    return true;
                }
            }
        }
        return false;
    }

    public static bool JumpPressed()
    {
        return Input.GetKeyDown(JumpKey) || Input.GetKeyDown(JumpKey1);
    }
    public static bool JumpHeld()
    {
        return Input.GetKey(JumpKey) || Input.GetKey(JumpKey1);
    }
    public static bool JumpReleased()
    {
        return Input.GetKeyUp(JumpKey) || Input.GetKeyUp(JumpKey1);
    }

    public static bool UpPressed()
    {
        return Input.GetKeyDown(UpKey) || Input.GetKeyDown(UpKey1);
    }
    public static bool UpHeld()
    {
        return Input.GetKey(UpKey) || Input.GetKey(UpKey1);
    }
    public static bool UpReleased()
    {
        return Input.GetKeyUp(UpKey) || Input.GetKeyUp(UpKey1);
    }

    public static bool DownPressed()
    {
        return Input.GetKeyDown(DownKey) || Input.GetKeyDown(DownKey1);
    }
    public static bool DownHeld()
    {
        return Input.GetKey(DownKey) || Input.GetKey(DownKey1);
    }
    public static bool DownReleased()
    {
        return Input.GetKeyUp(DownKey) || Input.GetKeyUp(DownKey1);
    }

    public static bool LeftPressed()
    {
        return Input.GetKeyDown(LeftKey) || Input.GetKeyDown(LeftKey1);
    }
    public static bool LeftHeld()
    {
        return Input.GetKey(LeftKey) || Input.GetKey(LeftKey1);
    }
    public static bool LeftReleased()
    {
        return Input.GetKeyUp(LeftKey) || Input.GetKeyUp(LeftKey1);
    }

    public static bool RightPressed()
    {
        return Input.GetKeyUp(RightKey) || Input.GetKeyUp(RightKey1);
    }
    public static bool RightHeld()
    {
        return Input.GetKey(RightKey) || Input.GetKey(RightKey1);
    }
    public static bool RightReleased()
    {
        return Input.GetKeyDown(RightKey) || Input.GetKeyDown(RightKey1);
    }

    public static bool SlashPressed()
    {
        return Input.GetMouseButtonDown(slashMB);
    }
    public static bool SlashHeld()
    {
        return Input.GetMouseButton(slashMB);
    }
    public static bool SlashReleased()
    {
        return Input.GetMouseButtonUp(slashMB);
    }

    public static bool DSlashPressed()
    {
        return Input.GetMouseButtonDown(dslashMB);
    }
    public static bool DSlashHeld()
    {
        return Input.GetMouseButton(dslashMB);
    }
    public static bool DSlashReleased()
    {
        return Input.GetMouseButtonUp(dslashMB);
    }

    public static bool DashRollPressed()
    {
        return Input.GetKeyDown(DashRollKey) || Input.GetKeyDown(DashRollKey1) || Input.GetMouseButtonDown(rollMB);
    }
    public static bool DashRollHeld()
    {
        return Input.GetKey(DashRollKey) || Input.GetKey(DashRollKey1) || Input.GetMouseButton(rollMB);
    }

    static Vector2 vect2;
    public static Vector2 GetVectorInput()
    {
        if (UpHeld())
        {
            if (LeftHeld())
            {
                vect2.x = -.707f;
                vect2.y = .707f;
            }
            else if (RightHeld())
            {
                vect2.x = .707f;
                vect2.y = .707f;
            }
            else
            {
                vect2.x = 0;
                vect2.y = 1;
            }
        }
        else if (DownHeld())
        {
            if (LeftHeld())
            {
                vect2.x = -.707f;
                vect2.y = -.707f;
            }
            else if (RightHeld())
            {
                vect2.x = .707f;
                vect2.y = -.707f;
            }
            else
            {
                vect2.x = 0;
                vect2.y = -1;
            }
        }
        else
        {
            if (LeftHeld())
            {
                vect2.x = -1;
                vect2.y = 0;
            }
            else if (RightHeld())
            {
                vect2.x = 1;
                vect2.y = 0;
            }
            else
            {
                return Vector2.zero;
            }
        }

        return vect2;
    }
}
