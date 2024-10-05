using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] int damage, entityID;
    [SerializeField] Collider2D col;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            col.GetComponent<HPEntity>().TakeDamage(damage, entityID);
        }        
    }
}
