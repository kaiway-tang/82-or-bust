using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    public int touchCount;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == Layers.TERRAIN || col.gameObject.layer == Layers.HURTBOX)
        {
            Debug.Log("Touching: " + col.gameObject);
            touchCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == Layers.TERRAIN || col.gameObject.layer == Layers.HURTBOX)
        {
            touchCount--;
        }
    }
}