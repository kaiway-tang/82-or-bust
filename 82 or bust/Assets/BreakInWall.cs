using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakInWall : HPEntity
{
    [SerializeField] GameObject damageFX;
    [SerializeField] int trauma;

    public override int TakeDamage(int amount, int sourceID)
    {
        int result = base.TakeDamage(amount, sourceID);
        if (result != HPEntity.IGNORED)
        {
            Instantiate(damageFX, transform.position, Quaternion.identity);
            CameraManager.SetTrauma(trauma);
            if (result == HPEntity.DEAD)
            {
                AudioController.Instance.SetLowPassCutoffFrequency(22000f);
                Destroy(baseObj);
            }
        }
        return result;
    }
}
