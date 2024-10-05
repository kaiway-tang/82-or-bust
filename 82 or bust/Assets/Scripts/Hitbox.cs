using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] int damage, entityID;
    [SerializeField] Collider2D hitboxCol;
    public Transform trfm;

    protected void Start()
    {
        if (!trfm) { trfm = transform; }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            col.GetComponent<HPEntity>().TakeDamage(damage, entityID);
        }        
    }

    int activeTmr;
    public void Activate(int duration = -1)
    {
        hitboxCol.enabled = true;
        activeTmr = duration;
    }

    public void Deactivate()
    {
        hitboxCol.enabled = false;
        activeTmr = 0;
    }

    protected void FixedUpdate()
    {
        if (activeTmr > 0)
        {
            activeTmr--;
            if (activeTmr < 1)
            {
                hitboxCol.enabled = false;
            }
        }        
    }
}
