using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrobingText : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] float fadeRate;
    bool fadingIn;
    [SerializeField] float activationX;

    private void FixedUpdate()
    {
        if (Player.self.trfm.position.x < activationX) {
            Tools.SetAlpha(rend, 0);
            return;
        }
        if (fadingIn)
        {
            Tools.AddAlpha(rend, fadeRate);
            if (rend.color.a > .99f) { fadingIn = false; }
        }
        else
        {
            Tools.AddAlpha(rend, -fadeRate);
            if (rend.color.a < .3f) { fadingIn = true; }
        }
    }
}
