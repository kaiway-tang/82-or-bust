using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    public const int HITBOX = 6, HURTBOX = 7, TERRAIN = 8;

    public static bool AnyCollision(int layer)
    {
        return layer >= HITBOX && layer <= TERRAIN;
    }
}
