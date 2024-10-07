using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsController : MonoBehaviour
{
    [SerializeField] ParticleSystem dodgeFX;
    [SerializeField] TrailRenderer dslashTrail;
    
    public void PlayDodgeFX()
    {
        dodgeFX.Play();
    }

    public void PlayDSlashFX()
    {
        dslashTrail.emitting = true;
        dslashTmr = 6;
    }

    void Start()
    {
        
    }

    int dslashTmr;
    private void FixedUpdate()
    {
        if (dslashTmr > 0)
        {
            dslashTmr--;
            if (dslashTmr < 1)
            {
                dslashTrail.emitting = false;
            }
        }
    }
}
