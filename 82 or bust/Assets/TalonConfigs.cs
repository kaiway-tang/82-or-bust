using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalonConfigs : MonoBehaviour
{
    [SerializeField] Transform talonConfigs;
    [SerializeField] Transform leftTalon, rightTalon;
    [SerializeField] Transform lTalonDodgeStart, lTalonDodgeEnd, rTalonDodgeStart, rTalonDodgeEnd;

    Transform trfm;
    private void Start()
    {
        trfm = transform;
    }
    public void SetDodgeTalons()
    {
        return;
        Vector3 mouseVect = CursorObj.trfm.position - trfm.position;
        trfm.up = mouseVect;

        leftTalon.rotation = lTalonDodgeStart.rotation;
        rightTalon.rotation = rTalonDodgeStart.rotation;
        talonConfigs.up = mouseVect;

        dodgeTmr = 0;
    }

    int dodgeTmr;
    private void FixedUpdate()
    {
        trfm.up = CursorObj.trfm.position - trfm.position;

        if (dodgeTmr > 0)
        {
            dodgeTmr--;
            Tools.LerpRotation(leftTalon, lTalonDodgeEnd.localRotation.z, 0.15f);
            Tools.LerpRotation(rightTalon, rTalonDodgeEnd.localRotation.z, 0.15f);
        }
    }
}
