using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    [SerializeField] Transform _emptyTrfm;
    static Transform emptyTrfm;
    static Vector3 vect3;
    static float tempFloat;

    public static Transform playerTrfm;

    public static void Init(Transform pPlayerTrfm)
    {
        playerTrfm = pPlayerTrfm;
    }

    private void Awake()
    {
        emptyTrfm = _emptyTrfm;
    }

    public static Vector2 RandomOnUnitCircle()
    {
        tempFloat = Random.Range(0f, 360f);
        vect3.x = Mathf.Sin(tempFloat);
        vect3.y = Mathf.Cos(tempFloat);

        return vect3;
    }


    public static bool InDistanceToPlayer(Vector2 pos, float distance)
    {
        return InDistance(pos, playerTrfm.position, distance);
    }

    public static bool InDistance(Vector2 pos1, Vector2 pos2, float distance)
    {
        return BoxDist(pos1, pos2) <= distance && (pos1 - pos2).sqrMagnitude <= distance * distance;
    }

    public static float BoxDist(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Max(Mathf.Abs(pos1.x - pos2.x), Mathf.Abs(pos1.y - pos2.y));
    }

    #region ALPHA
    static Color col;
    public static void AddAlpha(SpriteRenderer rend, float a, float min = 0, float max = 1)
    {
        tempFloat = rend.color.a + a;
        tempFloat = Mathf.Min(tempFloat, max);
        tempFloat = Mathf.Max(tempFloat, min);

        SetAlpha(rend, tempFloat);
    }

    public static void SetAlpha(SpriteRenderer rend, float a)
    {
        col = rend.color;
        col.a = a;
        rend.color = col;
    }
    #endregion

    #region RAYCASTING

    public static int terrainMask = 1 << 8, hurtMask = 1 << 7;
    public static bool LineOfSight(Vector2 pos1, Vector2 pos2)
    {
        return !Physics2D.Linecast(pos1, pos2, terrainMask);
    }

    public static bool PlayerVisible(Vector2 pos)
    {
        return LineOfSight(pos, playerTrfm.position);
    }

    #endregion

    #region ROTATION

    public static void LerpRotation(Transform trfm, float targetz, float rate)
    {
        vect3 = trfm.eulerAngles;
        vect3.z = RotationalLerp(vect3.z, targetz, rate);
        //vect3.z += (targetz - vect3.z) * rate;
        trfm.eulerAngles = vect3;
    }

    public static void FacePosition(Transform trfm, Vector3 targetPos, float rate, float offset = 0)
    {
        emptyTrfm.position = trfm.position;
        emptyTrfm.localEulerAngles = Vector3.zero;
        emptyTrfm.right = targetPos - trfm.position;
        if (offset != 0) { emptyTrfm.Rotate(Vector3.forward * offset); }

        vect3 = trfm.localEulerAngles;
        vect3.z = RotationalLerp(vect3.z, emptyTrfm.localEulerAngles.z, rate);
        trfm.localEulerAngles = vect3;
    }

    public static float RotationalLerp(float start, float dest, float rate)
    {
        if (Mathf.Abs(dest - start) < 180)
        {
            return start + (dest - start) * rate;
        }
        else
        {
            if (dest > start)
            {
                return (start + (dest - start - 360) * rate);
            }
            else
            {
                return (start + (360 - start + dest) * rate) % 360;
            }

        }
    }

    #endregion
}
