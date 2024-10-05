using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectDodge : HPEntity
{
    [SerializeField] GameObject perfectDodgeFX;
    [SerializeField] int window;

    new void FixedUpdate()
    {
        base.FixedUpdate();
        window--;
        if (window < 1) { Destroy(gameObject); }
    }

    public override int TakeDamage(int amount, int sourceID)
    {
        int result = base.TakeDamage(amount, sourceID);
        if (result == DEAD)
        {
            Player.self.PerfectDodged();
            Instantiate(perfectDodgeFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        return result;
    }
}
