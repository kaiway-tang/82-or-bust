using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsController : MonoBehaviour
{
    [SerializeField] ParticleSystem dodgeFX;
    [SerializeField] GameObject dodgeRingFX, pDodgeFX;
    [SerializeField] TrailRenderer dslashTrail;

    [SerializeReference] Transform mousePtr;
    Transform trfm;

    [SerializeField] TalonConfigs talons;
    [SerializeField] Transform spriteTrfm;

    public void PlayDodgeFX()
    {
        dodgeFX.Play();
        Vector3 mouseVect = CursorObj.trfm.position - trfm.position;
        mousePtr.up = mouseVect;
        Instantiate(dodgeRingFX, trfm.position + mouseVect.normalized * -0.2f, trfm.rotation).transform.up = mouseVect;
        talons.SetDodgeTalons();
    }

    public void PlayPDodgeFX()
    {
        Instantiate(pDodgeFX, trfm.position, Quaternion.identity);
    }

    public void PlayDSlashFX()
    {
        talons.SetDSlashTalons();
        dslashTrail.emitting = true;
        dslashTmr = 6;
    }

    void Start()
    {
        trfm = transform;
    }

    int dslashTmr;

    private void FixedUpdate()
    {
        spriteTrfm.right = CursorObj.trfm.position - trfm.position;
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
