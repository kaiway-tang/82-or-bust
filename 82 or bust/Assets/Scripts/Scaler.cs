using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] Transform scalerTrfm;
    static Vector3 vect3;

    public void SetScale(float percentage)
    {
        vect3 = scalerTrfm.localScale;
        vect3.x = percentage;
        scalerTrfm.localScale = vect3;
    }

    [SerializeReference] float targetScale, lerpRate;
    public void SetTargetScale(float percentage, float pLerpRate = 0.1f)
    {
        targetScale = percentage;
        lerpRate = pLerpRate;
        isLerping = true;
    }

    [SerializeReference] bool isLerping;
    private void FixedUpdate()
    {
        if (isLerping)
        {
            SetScale((1 - lerpRate) * scalerTrfm.localScale.x + lerpRate * targetScale);
            if (Mathf.Abs(scalerTrfm.localScale.x - targetScale) < 0.01f)
            {
                SetScale(targetScale);
                isLerping = false;
            }
        }
    }
}
