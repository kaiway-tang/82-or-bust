using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalonConfigs : MonoBehaviour
{
    [SerializeField] Transform talonConfigs;
    [SerializeField] Transform leftTalon, rightTalon;
    [SerializeField] Transform lTalonDodgeStart, lTalonDodgeEnd, rTalonDodgeStart, rTalonDodgeEnd;

    [SerializeField] int talonState = 0;
    const int IDLE = 0, DODGEFORM = 1, DSLASHFORM = 2;

    Transform trfm;
    private void Start()
    {
        trfm = transform;
    }
    public void SetDodgeTalons()
    {
        Vector3 mouseVect = CursorObj.trfm.position - trfm.position;
        trfm.up = mouseVect;

        leftTalon.rotation = rTalonDodgeEnd.rotation;
        rightTalon.rotation = lTalonDodgeEnd.rotation;
        talonConfigs.up = mouseVect;

        dodgeTmr = 19;
        dslashTmr = 0;
        talonState = DODGEFORM;
        talonConfigs.localScale = new Vector3(1.3f,1.3f,1);
    }

    public void SetDSlashTalons()
    {
        Vector3 mouseVect = CursorObj.trfm.position - trfm.position;
        trfm.up = mouseVect;

        leftTalon.rotation = rTalonDodgeStart.rotation;
        rightTalon.rotation = lTalonDodgeStart.rotation;
        talonConfigs.up = mouseVect;

        dslashTmr = 16;
        dodgeTmr = 0;
        talonState = DSLASHFORM;
        talonConfigs.localScale = new Vector3(1.8f, 1.8f, 1);
    }

    int dodgeTmr, dslashTmr;
    private void FixedUpdate()
    {
        if (talonState == IDLE)
        {
            leftTalon.Rotate(Vector3.forward * 14);
            rightTalon.Rotate(Vector3.forward * -11);
        }
        else if (talonState == DODGEFORM)
        {
            if (dodgeTmr > 0)
            {
                dodgeTmr--;
                if (dodgeTmr == 0) { talonState = IDLE;
                    talonConfigs.localScale = new Vector3(1.2f, 1.2f, 1);
                }
                //Tools.LerpRotation(leftTalon, lTalonDodgeEnd.localRotation.z, 0.15f);
                //Tools.LerpRotation(rightTalon, rTalonDodgeEnd.localRotation.z, 0.15f);
            }
        }
        else if (talonState == DSLASHFORM)
        {
            //trfm.up = CursorObj.trfm.position - trfm.position;

            if (dslashTmr > 0)
            {
                dslashTmr--;
                if (dslashTmr == 0) { talonState = IDLE;
                    talonConfigs.localScale = new Vector3(1.2f, 1.2f, 1);
                }
            }
        }


    }
}
